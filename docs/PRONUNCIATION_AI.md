# Pronunciation AI — Technical Flow

## Overview
The pronunciation feature is the **#1 differentiator** of EnglishAI. Users record their voice, the system performs Speech-to-Text with phoneme-level analysis, scores pronunciation accuracy, and provides visual feedback with waveform comparison.

---

## Architecture Flow

```
┌──────────┐    WebSocket/SignalR     ┌──────────────┐
│  Client   │ ──── audio stream ────▶ │  API Server   │
│  (Next.js)│                         │  (ASP.NET)    │
│           │ ◀── partial results ─── │               │
└──────────┘                         └──────┬───────┘
                                            │
                                    ┌───────▼────────┐
                                    │  Audio Pipeline  │
                                    │                  │
                                    │ 1. Validate WAV  │
                                    │ 2. Noise reduce  │
                                    │ 3. Normalize     │
                                    └───────┬──────────┘
                                            │
                          ┌─────────────────┼─────────────────┐
                          │                 │                   │
                   ┌──────▼──────┐  ┌──────▼──────┐   ┌──────▼──────┐
                   │ Azure Speech │  │   Whisper    │   │  Waveform   │
                   │ Pronunciation│  │   STT        │   │  Analysis   │
                   │ Assessment   │  │  (fallback)  │   │             │
                   └──────┬──────┘  └──────┬──────┘   └──────┬──────┘
                          │                 │                   │
                          └─────────────────┼───────────────────┘
                                            │
                                    ┌───────▼────────┐
                                    │  Score Engine   │
                                    │                  │
                                    │ - Phoneme match  │
                                    │ - Fluency calc   │
                                    │ - AI feedback    │
                                    └───────┬──────────┘
                                            │
                                    ┌───────▼────────┐
                                    │  Store + Return  │
                                    │  to Client       │
                                    └──────────────────┘
```

---

## Implementation Details

### Step 1: Client-Side Audio Recording

```typescript
// Frontend: useAudioRecorder hook
const useAudioRecorder = () => {
  const mediaRecorderRef = useRef<MediaRecorder | null>(null);
  const chunksRef = useRef<Blob[]>([]);

  const startRecording = async () => {
    const stream = await navigator.mediaDevices.getUserMedia({
      audio: {
        channelCount: 1,           // mono
        sampleRate: 16000,         // 16kHz (Azure Speech requirement)
        echoCancellation: true,
        noiseSuppression: true,
      }
    });

    const mediaRecorder = new MediaRecorder(stream, {
      mimeType: 'audio/webm;codecs=opus'  // or audio/wav
    });

    mediaRecorder.ondataavailable = (e) => {
      chunksRef.current.push(e.data);
      // Optional: stream chunks via SignalR for real-time feedback
      signalRConnection.invoke('SendAudioChunk', e.data);
    };

    mediaRecorder.start(250); // 250ms chunks for streaming
  };

  const stopRecording = async (): Promise<Blob> => {
    // Combine chunks → single audio blob
    const blob = new Blob(chunksRef.current, { type: 'audio/webm' });
    // Convert to WAV if needed (via AudioContext)
    return await convertToWav(blob);
  };
};
```

### Step 2: Audio Upload to Backend

```csharp
// API/Controllers/PronunciationController.cs
[HttpPost("sessions/{sessionId}/attempts")]
[Authorize]
[RequestSizeLimit(10_000_000)] // 10MB max
public async Task<ActionResult<PronunciationAttemptDto>> SubmitAttempt(
    Guid sessionId,
    [FromForm] IFormFile audio)
{
    // Validate audio file
    if (audio.Length == 0 || audio.Length > 10_000_000)
        throw new AppValidationException("Audio file invalid");

    var allowedTypes = new[] { "audio/wav", "audio/webm", "audio/mp4", "audio/ogg" };
    if (!allowedTypes.Contains(audio.ContentType))
        throw new AppValidationException("Unsupported audio format");

    var command = new EvaluatePronunciationCommand
    {
        SessionId = sessionId,
        AudioStream = audio.OpenReadStream(),
        AudioContentType = audio.ContentType
    };

    return Ok(await _mediator.Send(command));
}
```

### Step 3: Audio Processing Pipeline

```csharp
// Infrastructure/Services/Speech/AudioProcessingService.cs
public class AudioProcessingService : IAudioProcessingService
{
    public async Task<ProcessedAudio> ProcessAsync(Stream audioStream, string contentType)
    {
        // 1. Convert to WAV 16kHz mono if not already
        var wavStream = await ConvertToWavAsync(audioStream, contentType);

        // 2. Calculate duration
        var duration = GetAudioDuration(wavStream);
        if (duration > TimeSpan.FromSeconds(30))
            throw new AppValidationException("Audio too long (max 30s)");

        // 3. Generate waveform data for visualization
        var waveformData = ExtractWaveformData(wavStream, samplePoints: 200);

        wavStream.Position = 0;
        return new ProcessedAudio
        {
            WavStream = wavStream,
            DurationSeconds = duration.TotalSeconds,
            WaveformData = waveformData
        };
    }

    private float[] ExtractWaveformData(Stream wavStream, int samplePoints)
    {
        // Read WAV PCM data
        // Downsample to `samplePoints` values for frontend visualization
        // Normalize to 0.0 - 1.0 range
        var samples = ReadPcmSamples(wavStream);
        var chunkSize = samples.Length / samplePoints;
        var waveform = new float[samplePoints];

        for (int i = 0; i < samplePoints; i++)
        {
            var chunk = samples.Skip(i * chunkSize).Take(chunkSize);
            waveform[i] = chunk.Max(s => Math.Abs(s)); // peak amplitude per chunk
        }

        return waveform;
    }
}
```

### Step 4: Azure Speech Pronunciation Assessment

```csharp
// Infrastructure/Services/Speech/AzureSpeechService.cs
public class AzureSpeechService : IPronunciationAssessmentService
{
    private readonly SpeechConfig _speechConfig;

    public AzureSpeechService(IOptions<AzureSpeechSettings> settings)
    {
        _speechConfig = SpeechConfig.FromSubscription(
            settings.Value.SubscriptionKey,
            settings.Value.Region
        );
        _speechConfig.SpeechRecognitionLanguage = "en-US";
    }

    public async Task<PronunciationAssessmentResult> EvaluateAsync(
        Stream audioStream,
        string referenceText,
        CancellationToken ct)
    {
        // Configure pronunciation assessment
        var pronunciationConfig = new PronunciationAssessmentConfig(
            referenceText: referenceText,
            gradingSystem: GradingSystem.HundredMark,
            granularity: Granularity.Phoneme,   // phoneme-level detail!
            enableMiscue: true                   // detect missing/extra words
        );

        using var audioConfig = AudioConfig.FromStreamInput(
            new PushAudioInputStream(AudioStreamFormat.GetWaveFormatPCM(16000, 16, 1))
        );

        using var recognizer = new SpeechRecognizer(_speechConfig, audioConfig);
        pronunciationConfig.ApplyTo(recognizer);

        // Push audio data
        var pushStream = (PushAudioInputStream)audioConfig.GetProperty("audioStream");
        await PushAudioToStream(audioStream, pushStream);

        // Get result
        var result = await recognizer.RecognizeOnceAsync();

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            var assessment = PronunciationAssessmentResult.FromResult(result);
            return MapToOurModel(assessment, result);
        }

        throw new SpeechRecognitionException($"Recognition failed: {result.Reason}");
    }

    private PronunciationAssessmentResult MapToOurModel(
        PronunciationAssessmentResult azureResult,
        SpeechRecognitionResult rawResult)
    {
        return new PronunciationAssessmentResult
        {
            RecognizedText = rawResult.Text,
            AccuracyScore = azureResult.AccuracyScore,
            FluencyScore = azureResult.FluencyScore,
            CompletenessScore = azureResult.CompletenessScore,
            PronunciationScore = azureResult.PronunciationScore,
            Words = azureResult.Words.Select(w => new WordScore
            {
                Word = w.Word,
                AccuracyScore = w.AccuracyScore,
                ErrorType = w.ErrorType, // None, Omission, Insertion, Mispronunciation
                Phonemes = w.Phonemes.Select(p => new PhonemeScore
                {
                    Phoneme = p.Phoneme,
                    Score = p.AccuracyScore,
                    Offset = p.Offset,
                    Duration = p.Duration
                }).ToList()
            }).ToList()
        };
    }
}
```

### Step 5: Whisper Fallback (if Azure unavailable)

```csharp
// Infrastructure/Services/Speech/WhisperService.cs
public class WhisperService : IPronunciationAssessmentService
{
    private readonly HttpClient _httpClient;

    public async Task<PronunciationAssessmentResult> EvaluateAsync(
        Stream audioStream, string referenceText, CancellationToken ct)
    {
        // 1. Transcribe with Whisper API
        var transcription = await TranscribeAsync(audioStream, ct);

        // 2. Compare transcription with reference using Levenshtein + phoneme mapping
        var comparison = CompareTexts(referenceText, transcription.Text);

        // 3. Use word-level timestamps for fluency scoring
        var fluencyScore = CalculateFluencyScore(transcription.Words);

        // 4. Map to phonemes using CMU Pronouncing Dictionary
        var phonemeScores = MapToPhonemeScores(comparison, transcription.Words);

        return new PronunciationAssessmentResult
        {
            RecognizedText = transcription.Text,
            AccuracyScore = comparison.SimilarityPercent,
            FluencyScore = fluencyScore,
            CompletenessScore = comparison.CompletenessPercent,
            PronunciationScore = (comparison.SimilarityPercent + fluencyScore) / 2,
            Words = phonemeScores
        };
    }
}
```

### Step 6: AI Feedback Generation

```csharp
// Infrastructure/Services/AI/PronunciationFeedbackService.cs
public class PronunciationFeedbackService : IPronunciationFeedbackService
{
    private readonly IOpenAiService _openAi;

    public async Task<string> GenerateFeedbackAsync(
        PronunciationAssessmentResult result,
        string userLevel,
        CancellationToken ct)
    {
        var weakPhonemes = result.Words
            .SelectMany(w => w.Phonemes)
            .Where(p => p.Score < 60)
            .GroupBy(p => p.Phoneme)
            .OrderBy(g => g.Average(p => p.Score))
            .Take(3)
            .ToList();

        var prompt = $"""
            You are an English pronunciation coach for a {userLevel} level Vietnamese learner.
            
            Target text: "{result.RecognizedText}"
            Overall score: {result.PronunciationScore}/100
            
            Weak phonemes:
            {string.Join("\n", weakPhonemes.Select(g =>
                $"- /{g.Key}/ (avg score: {g.Average(p => p.Score):F0})"))}
            
            Mispronounced words:
            {string.Join("\n", result.Words.Where(w => w.ErrorType == "Mispronunciation")
                .Select(w => $"- '{w.Word}' (score: {w.AccuracyScore})"))}
            
            Give a brief, encouraging feedback (2-3 sentences) in simple English.
            Focus on the most impactful improvement tip.
            Include one specific mouth/tongue position tip for the weakest phoneme.
            If the user is Vietnamese, mention common Vietnamese-English phoneme confusion.
            """;

        return await _openAi.GetCompletionAsync(prompt, maxTokens: 200, ct: ct);
    }
}
```

### Step 7: Waveform Comparison Data

```csharp
// The response includes waveform data for frontend visualization
public class WaveformComparisonData
{
    // Reference waveform (from TTS)
    public float[] ReferenceAmplitudes { get; set; }  // 200 data points
    public float ReferenceDuration { get; set; }

    // User waveform
    public float[] UserAmplitudes { get; set; }        // 200 data points
    public float UserDuration { get; set; }

    // Alignment data (Dynamic Time Warping)
    public AlignmentPoint[] AlignmentPath { get; set; }

    // Word boundaries for highlighting
    public WordBoundary[] ReferenceWordBoundaries { get; set; }
    public WordBoundary[] UserWordBoundaries { get; set; }
}

public class AlignmentPoint
{
    public int ReferenceIndex { get; set; }
    public int UserIndex { get; set; }
    public float Cost { get; set; }
}
```

---

## Real-time Streaming (SignalR)

For real-time feedback during recording:

```csharp
// API/Hubs/PronunciationHub.cs
public class PronunciationHub : Hub
{
    private readonly IPronunciationAssessmentService _pronunciationService;
    private readonly ConcurrentDictionary<string, AudioAccumulator> _accumulators = new();

    public async Task StartSession(Guid sessionId, string referenceText)
    {
        var connectionId = Context.ConnectionId;
        _accumulators[connectionId] = new AudioAccumulator(referenceText);
        await Clients.Caller.SendAsync("SessionStarted", sessionId);
    }

    public async Task SendAudioChunk(byte[] audioData)
    {
        var connectionId = Context.ConnectionId;
        if (!_accumulators.TryGetValue(connectionId, out var accumulator))
            return;

        accumulator.AddChunk(audioData);

        // Real-time volume level for visual feedback
        var volumeLevel = CalculateRmsVolume(audioData);
        await Clients.Caller.SendAsync("VolumeLevel", volumeLevel);

        // Partial recognition (every ~1 second)
        if (accumulator.ShouldProcessPartial())
        {
            var partialResult = await _pronunciationService.GetPartialResultAsync(
                accumulator.GetCurrentAudio());
            await Clients.Caller.SendAsync("PartialResult", new
            {
                partialResult.RecognizedText,
                partialResult.PartialScore
            });
        }
    }

    public async Task EndSession()
    {
        var connectionId = Context.ConnectionId;
        if (!_accumulators.TryRemove(connectionId, out var accumulator))
            return;

        // Full assessment
        var fullResult = await _pronunciationService.EvaluateAsync(
            accumulator.GetFinalAudio(),
            accumulator.ReferenceText,
            CancellationToken.None);

        await Clients.Caller.SendAsync("FinalResult", fullResult);
    }
}
```

---

## Minimal Pairs Drill

Special pronunciation exercise comparing similar sounds:

```csharp
// Domain/Entities/MinimalPair.cs
public class MinimalPair
{
    public string Word1 { get; set; }          // "ship"
    public string Phoneme1 { get; set; }       // "ʃɪp"
    public string Word2 { get; set; }          // "sheep"
    public string Phoneme2 { get; set; }       // "ʃiːp"
    public string TargetPhoneme1 { get; set; } // "ɪ"
    public string TargetPhoneme2 { get; set; } // "iː"
    public string AudioUrl1 { get; set; }
    public string AudioUrl2 { get; set; }
    public string CefrLevel { get; set; }
}

// Common Vietnamese learner minimal pairs:
// /ɪ/ vs /iː/  → ship/sheep, bit/beat, sit/seat
// /æ/ vs /e/   → bad/bed, man/men, sat/set
// /θ/ vs /t/   → think/tink, three/tree, math/mat
// /ð/ vs /d/   → then/den, they/day, breathe/breed
// /ʃ/ vs /s/   → she/see, ship/sip, shoe/sue
// /r/ vs /l/   → right/light, read/lead, rock/lock
// /v/ vs /w/   → vine/wine, vest/west, vet/wet
```

---

## Storage Strategy

```
Azure Blob Storage / MinIO
├── audio/
│   ├── reference/           # TTS-generated reference audio
│   │   └── {hash}.mp3
│   ├── user-attempts/       # User recordings
│   │   └── {userId}/{attemptId}.wav
│   └── minimal-pairs/       # Pre-recorded minimal pair audio
│       └── {word}.mp3
```

Audio retention policy:
- Reference audio: permanent
- User attempts: 90 days (then delete audio, keep scores)
- Minimal pairs: permanent

---

## Performance Considerations

1. **Audio upload**: Max 10MB, 30 seconds. Compress on client side.
2. **Azure Speech API**: ~1-3s latency. Cache TTS reference audio.
3. **Waveform generation**: Pre-compute reference waveforms.
4. **SignalR**: Use MessagePack binary protocol for audio chunks.
5. **Rate limiting**: Max 60 pronunciation attempts per hour per user.
6. **Background processing**: Queue heavy analysis via background job if needed.

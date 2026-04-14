# AI Integration — EnglishAI

## AI Services Overview

| Feature | AI Provider | Model | Purpose |
|---------|------------|-------|---------|
| Pronunciation Assessment | Azure Cognitive Speech | - | Phoneme scoring |
| Reference Audio (TTS) | Azure Cognitive Speech | en-US-JennyNeural | Generate reference pronunciation |
| Speech-to-Text | Azure Speech / Whisper | whisper-1 | Transcribe user audio |
| Grammar Checker | OpenAI | gpt-4o-mini | Correct grammar errors |
| Writing Feedback | OpenAI | gpt-4o | Detailed writing analysis |
| AI Conversation | OpenAI | gpt-4o | Free chat + roleplay |
| Pronunciation Tips | OpenAI | gpt-4o-mini | Generate feedback |
| Content Generation | OpenAI | gpt-4o | Generate lessons, articles, flashcards |
| Reading Assistant | OpenAI | gpt-4o-mini | Answer questions about articles |

---

## Service Abstraction

```csharp
// Application/Common/Interfaces/IAiCompletionService.cs
public interface IAiCompletionService
{
    Task<string> GetCompletionAsync(string systemPrompt, string userMessage,
        int maxTokens = 500, float temperature = 0.7f, CancellationToken ct = default);

    IAsyncEnumerable<string> StreamCompletionAsync(string systemPrompt, string userMessage,
        int maxTokens = 1000, float temperature = 0.7f, CancellationToken ct = default);
}

// Application/Common/Interfaces/ISpeechService.cs
public interface ISpeechService
{
    Task<TranscriptionResult> TranscribeAsync(Stream audioStream, CancellationToken ct);
    Task<Stream> SynthesizeAsync(string text, string voice = "en-US-JennyNeural", CancellationToken ct = default);
}

// Application/Common/Interfaces/IPronunciationAssessmentService.cs
public interface IPronunciationAssessmentService
{
    Task<PronunciationAssessmentResult> EvaluateAsync(Stream audioStream, string referenceText, CancellationToken ct);
}
```

---

## AI Conversation System Prompts

### Free Chat
```
You are an English tutor for a {level} level learner (native language: Vietnamese).

Rules:
1. Keep your vocabulary and grammar at {level} CEFR level
2. If the user makes a grammar mistake, gently correct it in-context
3. Ask follow-up questions to keep the conversation going
4. Introduce 1-2 new vocabulary words per response, with brief explanation
5. Be encouraging and patient
6. Keep responses under 100 words
7. If the user writes in Vietnamese, respond in English and ask them to try in English
```

### Roleplay — Restaurant Waiter
```
You are a friendly waiter at "The Golden Spoon" restaurant in London.

Scenario objectives for the learner:
- Greet and get seated
- Ask about the menu
- Order food and drinks
- Ask about ingredients (allergies)
- Request the bill

Stay in character. Speak naturally as a British waiter would.
If the learner struggles, offer gentle hints.
Keep vocabulary at {level} level.
```

### Grammar Checker
```
You are an English grammar checker. Analyze the following text and return ONLY a JSON response:

{
  "corrections": [
    {
      "original": "exact original phrase",
      "corrected": "corrected version",
      "type": "grammar|spelling|punctuation|word_choice|style",
      "explanation": "brief explanation in simple English",
      "severity": "error|warning|suggestion"
    }
  ],
  "correctedText": "full corrected text",
  "overallLevel": "estimated CEFR level of writing"
}

Be thorough but prioritize actual errors over style preferences.
```

### Writing Feedback
```
You are an IELTS/TOEFL writing examiner. Evaluate the following text for a {level} learner.

Prompt: "{prompt}"
Text: "{text}"

Return JSON:
{
  "scores": {
    "overall": 0-100,
    "grammar": 0-100,
    "vocabulary": 0-100,
    "coherence": 0-100,
    "taskAchievement": 0-100
  },
  "correctedText": "full corrected version",
  "inlineCorrections": [
    {
      "offset": number,
      "length": number,
      "type": "grammar|vocabulary|coherence|style",
      "original": "...",
      "correction": "...",
      "explanation": "..."
    }
  ],
  "generalFeedback": "2-3 sentences of overall feedback",
  "improvementTips": ["tip1", "tip2", "tip3"],
  "vocabularySuggestions": [
    { "used": "good", "alternatives": ["excellent", "outstanding", "remarkable"] }
  ]
}
```

---

## Rate Limiting & Cost Control

```csharp
// Per-user AI usage limits
public class AiUsageLimits
{
    // Free tier
    public const int FreeConversationMessagesPerDay = 20;
    public const int FreePronunciationAttemptsPerDay = 30;
    public const int FreeWritingSubmissionsPerDay = 3;
    public const int FreeGrammarChecksPerDay = 10;

    // Premium tier
    public const int PremiumConversationMessagesPerDay = 200;
    public const int PremiumPronunciationAttemptsPerDay = 200;
    public const int PremiumWritingSubmissionsPerDay = 30;
    public const int PremiumGrammarChecksPerDay = 100;
}
```

Track usage in Redis:
```
Key: ai:usage:{userId}:{feature}:{date}
Value: count
TTL: 24h
```

```csharp
public class AiRateLimitingService
{
    private readonly IRedisCache _cache;

    public async Task<bool> CanUseFeatureAsync(Guid userId, string feature, string userRole)
    {
        var key = $"ai:usage:{userId}:{feature}:{DateTime.UtcNow:yyyy-MM-dd}";
        var count = await _cache.GetAsync<int>(key);
        var limit = GetLimit(feature, userRole);
        return count < limit;
    }

    public async Task IncrementUsageAsync(Guid userId, string feature)
    {
        var key = $"ai:usage:{userId}:{feature}:{DateTime.UtcNow:yyyy-MM-dd}";
        await _cache.IncrementAsync(key, TimeSpan.FromHours(25));
    }
}
```

---

## Token Cost Estimation

| Feature | Model | Avg Tokens/Call | Calls/Day (1000 users) | Cost/Day |
|---------|-------|-----------------|----------------------|----------|
| Grammar Check | gpt-4o-mini | 300 | 5000 | ~$0.75 |
| Writing Feedback | gpt-4o | 1000 | 1500 | ~$7.50 |
| Conversation | gpt-4o | 500 | 10000 | ~$50.00 |
| Pronunciation Tips | gpt-4o-mini | 200 | 3000 | ~$0.30 |
| Content Generation | gpt-4o | 2000 | 100 | ~$1.00 |
| **Total** | | | | **~$60/day** |

---

## Fallback Strategy

```csharp
// If primary AI service fails, fallback gracefully
public class ResilientAiService : IAiCompletionService
{
    private readonly IAiCompletionService _primary;   // OpenAI
    private readonly IAiCompletionService _fallback;  // Anthropic or local

    public async Task<string> GetCompletionAsync(...)
    {
        try
        {
            return await _primary.GetCompletionAsync(...);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            _logger.LogWarning("Primary AI service failed, using fallback");
            return await _fallback.GetCompletionAsync(...);
        }
    }
}
```

# SRS Algorithm — SM-2 Implementation

## Overview
EnglishAI uses a modified SM-2 (SuperMemo 2) algorithm for flashcard spaced repetition.

## SM-2 Formula

After each review, given quality `q` (0-5):

```
if q >= 3 (correct):
    if repetitions == 0:
        interval = 1 day
    elif repetitions == 1:
        interval = 6 days
    else:
        interval = previous_interval * ease_factor

    repetitions += 1
else (incorrect):
    repetitions = 0
    interval = 1 day

ease_factor = ease_factor + (0.1 - (5 - q) * (0.08 + (5 - q) * 0.02))
ease_factor = max(1.3, ease_factor)  // minimum 1.3

next_review = now + interval days
```

## Quality Scale Mapping

| Button | Quality | Meaning |
|--------|---------|---------|
| Again | 0 | Complete blackout, wrong answer |
| Hard | 2 | Correct but with significant difficulty |
| Good | 4 | Correct with some hesitation |
| Easy | 5 | Perfect, instant recall |

## C# Implementation

```csharp
// Domain/ValueObjects/SrsData.cs
public class SrsData : ValueObject
{
    public decimal EaseFactor { get; private set; }
    public int IntervalDays { get; private set; }
    public int Repetitions { get; private set; }
    public DateTime NextReviewAt { get; private set; }
    public DateTime? LastReviewedAt { get; private set; }

    public static SrsData CreateNew() => new()
    {
        EaseFactor = 2.5m,
        IntervalDays = 0,
        Repetitions = 0,
        NextReviewAt = DateTime.UtcNow
    };

    public SrsData Review(int quality)
    {
        if (quality < 0 || quality > 5)
            throw new ArgumentOutOfRangeException(nameof(quality));

        var newEf = EaseFactor + (0.1m - (5 - quality) * (0.08m + (5 - quality) * 0.02m));
        newEf = Math.Max(1.3m, newEf);

        int newInterval;
        int newReps;

        if (quality >= 3)
        {
            newReps = Repetitions + 1;
            newInterval = Repetitions switch
            {
                0 => 1,
                1 => 6,
                _ => (int)Math.Ceiling(IntervalDays * (double)newEf)
            };
        }
        else
        {
            newReps = 0;
            newInterval = 1;
        }

        return new SrsData
        {
            EaseFactor = newEf,
            IntervalDays = newInterval,
            Repetitions = newReps,
            NextReviewAt = DateTime.UtcNow.AddDays(newInterval),
            LastReviewedAt = DateTime.UtcNow
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EaseFactor;
        yield return IntervalDays;
        yield return Repetitions;
        yield return NextReviewAt;
    }
}
```

## Querying Due Cards

```csharp
// Get cards due for review, ordered by priority
var dueCards = await _context.Flashcards
    .Where(f => f.DeckId == deckId)
    .Where(f => !f.IsDeleted)
    .Where(f => f.NextReviewAt <= DateTime.UtcNow)
    .OrderBy(f => f.NextReviewAt)          // oldest due first
    .ThenBy(f => f.EaseFactor)             // hardest cards first
    .Take(limit)
    .ToListAsync(ct);
```

## Stats Calculation

```csharp
public class SrsStatsService
{
    public async Task<SrsStats> GetStatsAsync(Guid userId, Guid deckId)
    {
        var cards = await _context.Flashcards
            .Where(f => f.Deck.UserId == userId && f.DeckId == deckId && !f.IsDeleted)
            .ToListAsync();

        return new SrsStats
        {
            TotalCards = cards.Count,
            NewCards = cards.Count(c => c.Repetitions == 0),
            LearningCards = cards.Count(c => c.Repetitions > 0 && c.IntervalDays < 21),
            MatureCards = cards.Count(c => c.IntervalDays >= 21),
            DueToday = cards.Count(c => c.NextReviewAt <= DateTime.UtcNow),
            RetentionRate = CalculateRetentionRate(userId, deckId),
            ForecastNext7Days = CalculateForecast(cards, 7)
        };
    }
}
```

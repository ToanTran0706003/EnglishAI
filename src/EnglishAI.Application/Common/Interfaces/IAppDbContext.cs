using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Application abstraction over the EF Core DbContext.
/// </summary>
public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserExternalLogin> UserExternalLogins { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<UserSettings> UserSettings { get; }
    DbSet<UserStreak> UserStreaks { get; }
    DbSet<LearningPath> LearningPaths { get; }
    DbSet<LearningPathUnit> LearningPathUnits { get; }
    DbSet<Lesson> Lessons { get; }
    DbSet<UnitLesson> UnitLessons { get; }
    DbSet<LessonCompletion> LessonCompletions { get; }
    DbSet<FlashcardDeck> FlashcardDecks { get; }
    DbSet<Flashcard> Flashcards { get; }
    DbSet<FlashcardReview> FlashcardReviews { get; }
    DbSet<PronunciationSession> PronunciationSessions { get; }
    DbSet<PronunciationAttempt> PronunciationAttempts { get; }
    DbSet<ConversationSession> ConversationSessions { get; }
    DbSet<ConversationMessage> ConversationMessages { get; }
    DbSet<RoleplayScenario> RoleplayScenarios { get; }
    DbSet<WritingSubmission> WritingSubmissions { get; }
    DbSet<WritingFeedback> WritingFeedback { get; }
    DbSet<ReadingArticle> ReadingArticles { get; }
    DbSet<ReadingSession> ReadingSessions { get; }
    DbSet<ListeningExercise> ListeningExercises { get; }
    DbSet<DictationAttempt> DictationAttempts { get; }
    DbSet<GrammarTopic> GrammarTopics { get; }
    DbSet<GrammarQuestion> GrammarQuestions { get; }
    DbSet<GrammarQuizAttempt> GrammarQuizAttempts { get; }
    DbSet<XpTransaction> XpTransactions { get; }
    DbSet<Achievement> Achievements { get; }
    DbSet<UserAchievement> UserAchievements { get; }
    DbSet<LeaderboardEntry> LeaderboardEntries { get; }
    DbSet<VocabularyBank> VocabularyBank { get; }
    DbSet<DailyChallenge> DailyChallenges { get; }
    DbSet<DailyChallengeCompletion> DailyChallengeCompletions { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}


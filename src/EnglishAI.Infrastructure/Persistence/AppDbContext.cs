using EnglishAI.Application.Common.Interfaces;
using EnglishAI.Domain.Entities;
using EnglishAI.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace EnglishAI.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableInterceptor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableInterceptor)
        : base(options)
    {
        _auditableInterceptor = auditableInterceptor;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserExternalLogin> UserExternalLogins => Set<UserExternalLogin>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<UserStreak> UserStreaks => Set<UserStreak>();
    public DbSet<LearningPath> LearningPaths => Set<LearningPath>();
    public DbSet<LearningPathUnit> LearningPathUnits => Set<LearningPathUnit>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<UnitLesson> UnitLessons => Set<UnitLesson>();
    public DbSet<LessonCompletion> LessonCompletions => Set<LessonCompletion>();
    public DbSet<FlashcardDeck> FlashcardDecks => Set<FlashcardDeck>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<FlashcardReview> FlashcardReviews => Set<FlashcardReview>();
    public DbSet<PronunciationSession> PronunciationSessions => Set<PronunciationSession>();
    public DbSet<PronunciationAttempt> PronunciationAttempts => Set<PronunciationAttempt>();
    public DbSet<ConversationSession> ConversationSessions => Set<ConversationSession>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();
    public DbSet<RoleplayScenario> RoleplayScenarios => Set<RoleplayScenario>();
    public DbSet<WritingSubmission> WritingSubmissions => Set<WritingSubmission>();
    public DbSet<WritingFeedback> WritingFeedback => Set<WritingFeedback>();
    public DbSet<ReadingArticle> ReadingArticles => Set<ReadingArticle>();
    public DbSet<ReadingSession> ReadingSessions => Set<ReadingSession>();
    public DbSet<ListeningExercise> ListeningExercises => Set<ListeningExercise>();
    public DbSet<DictationAttempt> DictationAttempts => Set<DictationAttempt>();
    public DbSet<GrammarTopic> GrammarTopics => Set<GrammarTopic>();
    public DbSet<GrammarQuestion> GrammarQuestions => Set<GrammarQuestion>();
    public DbSet<GrammarQuizAttempt> GrammarQuizAttempts => Set<GrammarQuizAttempt>();
    public DbSet<XpTransaction> XpTransactions => Set<XpTransaction>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
    public DbSet<VocabularyBank> VocabularyBank => Set<VocabularyBank>();
    public DbSet<DailyChallenge> DailyChallenges => Set<DailyChallenge>();
    public DbSet<DailyChallengeCompletion> DailyChallengeCompletions => Set<DailyChallengeCompletion>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}


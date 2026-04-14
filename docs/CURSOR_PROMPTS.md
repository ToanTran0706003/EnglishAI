# Cursor Prompts — Copy & Paste Từng Bước

> Mỗi prompt dưới đây bạn copy paste vào Cursor chat. Chờ nó làm xong rồi mới paste prompt tiếp theo.

---

## 🔧 PROMPT 0.1 — Tạo Solution Structure

```
Create the .NET 8 solution structure for EnglishAI project. Follow Clean Architecture strictly.

1. Create solution file `EnglishAI.sln`
2. Create these projects:
   - `src/EnglishAI.Domain` (Class Library .NET 8) — NO external dependencies
   - `src/EnglishAI.Application` (Class Library .NET 8)
   - `src/EnglishAI.Infrastructure` (Class Library .NET 8)
   - `src/EnglishAI.Shared` (Class Library .NET 8) — NO dependencies
   - `src/EnglishAI.API` (ASP.NET Core Web API .NET 8)
   - `tests/EnglishAI.UnitTests` (xUnit)
   - `tests/EnglishAI.IntegrationTests` (xUnit)
   - `tests/EnglishAI.ArchTests` (xUnit)

3. Set project references:
   - Application → Domain
   - Infrastructure → Application, Domain
   - API → Application (NOT Infrastructure directly)
   - Shared → nothing
   - All test projects → all src projects

4. Add a `Directory.Build.props` at root with:
   - LangVersion=12, Nullable=enable, ImplicitUsings=enable
   - TreatWarningsAsErrors=true

5. Add `.gitignore` for .NET, `.editorconfig`

Do NOT install NuGet packages yet. Just create the empty structure.
```

---

## 🔧 PROMPT 0.2 — Install NuGet Packages

```
Install NuGet packages for the EnglishAI solution. Use the latest stable versions compatible with .NET 8.

**EnglishAI.Application:**
- MediatR (v12+)
- FluentValidation (v11+)
- FluentValidation.DependencyInjectionExtensions
- Mapster
- Microsoft.EntityFrameworkCore (for IAppDbContext interface only)

**EnglishAI.Infrastructure:**
- Npgsql.EntityFrameworkCore.PostgreSQL
- EFCore.NamingConventions
- Microsoft.EntityFrameworkCore.Tools
- StackExchange.Redis
- Serilog
- Serilog.Sinks.Seq
- Serilog.Extensions.Hosting
- Minio (MinIO SDK)
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- BCrypt.Net-Next
- System.IdentityModel.Tokens.Jwt

**EnglishAI.API:**
- Serilog.AspNetCore
- Swashbuckle.AspNetCore
- Microsoft.AspNetCore.SignalR
- AspNetCoreRateLimit

**Test projects:**
- xUnit
- FluentAssertions
- Moq
- Bogus
- Testcontainers
- Testcontainers.PostgreSql
- Microsoft.NET.Test.Sdk

Install via `dotnet add package` commands. Do NOT modify any source code.
```

---

## 🔧 PROMPT 0.3 — Domain Layer Base Classes

```
Create the Domain layer base classes and common types for EnglishAI.

In `src/EnglishAI.Domain/Common/`:
- `BaseEntity.cs` — abstract class with `Guid Id`, auto-generated
- `BaseAuditableEntity.cs` — extends BaseEntity, adds `DateTime CreatedAt`, `DateTime UpdatedAt`
- `ISoftDeletable.cs` — interface with `bool IsDeleted`, `DateTime? DeletedAt`
- `ValueObject.cs` — abstract class with `GetEqualityComponents()` pattern
- `IDomainEvent.cs` — marker interface extending MediatR.INotification

In `src/EnglishAI.Domain/Enums/`:
- `CefrLevel.cs` — enum: A1, A2, B1, B2, C1, C2
- `UserRole.cs` — enum: Learner, Premium, Admin
- `LessonType.cs` — enum: Grammar, Vocabulary, Pronunciation, Reading, Listening, Writing, Mixed
- `QuestionType.cs` — enum: MultipleChoice, FillBlank, Reorder, ErrorCorrection
- `ConversationSessionType.cs` — enum: FreeChat, Roleplay
- `PronunciationSessionType.cs` — enum: Word, Sentence, MinimalPair, FreeSpeech
- `WritingPromptType.cs` — enum: Essay, Email, Letter, Story, Summary, Free
- `AchievementCategory.cs` — enum: Streak, Lesson, Pronunciation, Vocabulary, Social, Writing
- `XpSource.cs` — enum: LessonComplete, FlashcardReview, StreakBonus, Pronunciation, Writing, Achievement, DailyChallenge, Conversation

In `src/EnglishAI.Domain/Exceptions/`:
- `DomainException.cs` — base domain exception

Keep everything clean, no external dependencies except MediatR.INotification for IDomainEvent.
```

---

## 🔧 PROMPT 0.4 — All Domain Entities

```
Create ALL domain entities for the EnglishAI project based on the database schema in `docs/database/DATABASE_SCHEMA.md`.

Create these entity files in `src/EnglishAI.Domain/Entities/`:

1. `User.cs` — id, email, normalizedEmail, passwordHash, displayName, avatarUrl, role (UserRole enum), emailConfirmed, isActive, lastLoginAt. Navigation: Profile, Settings, Streak, ExternalLogins, RefreshTokens
2. `UserExternalLogin.cs` — userId, provider, providerKey, providerEmail
3. `RefreshToken.cs` — userId, token, expiresAt, revokedAt, replacedBy, createdByIp
4. `UserProfile.cs` — userId, nativeLanguage, currentLevel (CefrLevel), targetLevel, dailyGoalMinutes, learningPurpose, weakSkills (List<string>), interests (List<string>), onboardingCompleted, timezone
5. `UserSettings.cs` — userId, notificationEnabled, dailyReminderTime, soundEnabled, darkMode, languageUi, autoPlayAudio, showPhonetic
6. `UserStreak.cs` — userId, currentStreak, longestStreak, lastActivityDate, freezeCount, totalDaysLearned
7. `LearningPath.cs` — userId, title, description, cefrLevel, isActive, progressPct. Nav: Units
8. `LearningPathUnit.cs` — learningPathId, title, description, sortOrder, isLocked, isCompleted, unitType (LessonType). Nav: UnitLessons
9. `Lesson.cs` — title, description, lessonType, cefrLevel, contentJson (string), durationMin, xpReward, difficulty, tags (List<string>), isPublished, isAiGenerated
10. `UnitLesson.cs` — unitId, lessonId, sortOrder
11. `LessonCompletion.cs` — userId, lessonId, score, timeSpentSec, xpEarned, completedAt, attemptNumber
12. `FlashcardDeck.cs` — userId, title, description, isPublic, cardCount, cefrLevel, tags. Implements ISoftDeletable. Nav: Cards
13. `Flashcard.cs` — deckId, front, back, phonetic, exampleSentence, audioUrl, imageUrl, partOfSpeech, easeFactor=2.5, intervalDays=0, repetitions=0, nextReviewAt, lastReviewedAt. Implements ISoftDeletable. Nav: Reviews
14. `FlashcardReview.cs` — flashcardId, userId, quality (0-5), timeSpentMs, wasCorrect, previousInterval, newInterval, reviewedAt
15. `PronunciationSession.cs` — userId, sessionType (PronunciationSessionType), targetText, targetPhonemes, cefrLevel, lessonId. Nav: Attempts
16. `PronunciationAttempt.cs` — sessionId, userId, audioUrl, audioDurationSec, recognizedText, recognizedPhonemes, accuracyScore, fluencyScore, completenessScore, pronunciationScore, phonemeScores (string/JSONB), wordScores (string/JSONB), feedbackText, waveformData (string/JSONB), attemptedAt
17. `ConversationSession.cs` — userId, sessionType, scenarioId, aiModel, systemPrompt, cefrLevel, messageCount, durationSec, grammarErrors, vocabularyUsed, sessionFeedback (string/JSONB), startedAt, endedAt. Nav: Messages
18. `ConversationMessage.cs` — sessionId, role, content, grammarIssues (string/JSONB), vocabLevel, audioUrl, tokenCount, sortOrder
19. `RoleplayScenario.cs` — title, description, category, cefrLevel, systemPrompt, starterMessage, objectives (List<string>), vocabularyHints (List<string>), isPublished
20. `WritingSubmission.cs` — userId, promptText, promptType, cefrLevel, userText, wordCount, lessonId. Nav: Feedback
21. `WritingFeedback.cs` — submissionId, overallScore, grammarScore, vocabularyScore, coherenceScore, taskAchievement, correctedText, inlineCorrections (string/JSONB), generalFeedback, improvementTips (string/JSONB), vocabularySuggestions (string/JSONB), aiModel
22. `ReadingArticle.cs` — title, content, summary, cefrLevel, category, wordCount, estimatedMin, vocabularyList (string/JSONB), comprehensionQuestions (string/JSONB), sourceUrl, imageUrl, isAiGenerated, isPublished
23. `ReadingSession.cs` — userId, articleId, timeSpentSec, progressPct, quizScore, wordsLookedUp (string/JSONB), completedAt
24. `ListeningExercise.cs` — title, audioUrl, transcript, cefrLevel, exerciseType, durationSec, category, questions (string/JSONB), isPublished
25. `DictationAttempt.cs` — userId, exerciseId, userText, accuracyPct, errors (string/JSONB), timeSpentSec, attemptedAt
26. `GrammarTopic.cs` — title, slug, explanation, examples (string/JSONB), cefrLevel, category, sortOrder, isPublished. Nav: Questions
27. `GrammarQuestion.cs` — topicId, questionType, questionText, options (string/JSONB), correctAnswer, explanation, difficulty
28. `GrammarQuizAttempt.cs` — userId, topicId, score, totalQuestions, correctCount, timeSpentSec, answers (string/JSONB), attemptedAt
29. `XpTransaction.cs` — userId, amount, source (XpSource), sourceId, description, balanceAfter
30. `Achievement.cs` — code, title, description, iconUrl, xpReward, category (AchievementCategory), conditionJson (string), sortOrder
31. `UserAchievement.cs` — userId, achievementId, unlockedAt
32. `LeaderboardEntry.cs` — userId, periodType, periodKey, totalXp, rank
33. `VocabularyBank.cs` — userId, word, definition, phonetic, partOfSpeech, exampleSentence, audioUrl, source, sourceId, masteryLevel=0, isFavorite
34. `DailyChallenge.cs` — challengeDate, title, description, challengeType, contentJson, xpReward
35. `DailyChallengeCompletion.cs` — userId, challengeId, score, completedAt

All entities extend BaseAuditableEntity. Use proper C# naming (PascalCase properties).
JSONB fields should be stored as string with comments indicating they're JSONB in PostgreSQL.
Use `= null!;` for required reference types. Use `= new List<T>();` for collections.
```

---

## 🔧 PROMPT 0.5 — Application Layer Base

```
Create the Application layer base infrastructure for EnglishAI.

1. `Application/Common/Interfaces/IAppDbContext.cs`
   - DbSet properties for ALL 35 entities
   - `Task<int> SaveChangesAsync(CancellationToken ct)`

2. `Application/Common/Interfaces/ICurrentUserService.cs`
   - Guid UserId, string Email, string Role, bool IsAuthenticated

3. `Application/Common/Interfaces/IAiCompletionService.cs`
   - GetCompletionAsync(systemPrompt, userMessage, maxTokens, temperature, ct)
   - StreamCompletionAsync(same params) returning IAsyncEnumerable<string>

4. `Application/Common/Interfaces/ISpeechService.cs`
   - TranscribeAsync(audioStream, ct)
   - SynthesizeAsync(text, voice, ct)

5. `Application/Common/Interfaces/IPronunciationAssessmentService.cs`
   - EvaluateAsync(audioStream, referenceText, ct)

6. `Application/Common/Interfaces/ICacheService.cs`
   - GetAsync<T>, SetAsync<T>, RemoveAsync, ExistsAsync, IncrementAsync

7. `Application/Common/Interfaces/IFileStorageService.cs`
   - UploadAsync(stream, fileName, contentType, ct)
   - GetUrlAsync(fileName)
   - DeleteAsync(fileName, ct)

8. `Application/Common/Models/PaginatedList.cs`
   - Generic paginated list with Items, Page, PageSize, TotalCount, TotalPages

9. `Application/Common/Models/ApiResponse.cs` and `ApiErrorResponse.cs`
   - Standard response wrappers

10. `Application/Common/Exceptions/`
    - NotFoundException, ValidationException (with FluentValidation errors), ForbiddenException, UnauthorizedException, RateLimitException

11. `Application/Common/Behaviors/`
    - LoggingBehavior, ValidationBehavior, PerformanceBehavior, CachingBehavior, TransactionBehavior
    - ICacheable interface (CacheKey, CacheDurationMinutes)

12. `Application/DependencyInjection.cs`
    - Register MediatR, FluentValidation (from assembly), all pipeline behaviors, Mapster

Include XML documentation comments on all public interfaces.
```

---

## 🔧 PROMPT 0.6 — Infrastructure: DbContext + Configurations

```
Create the Infrastructure persistence layer for EnglishAI.

1. `Infrastructure/Persistence/AppDbContext.cs`
   - Implement IAppDbContext
   - DbSet for all 35 entities
   - In OnModelCreating: ApplyConfigurationsFromAssembly, UseSnakeCaseNamingConvention()
   - Register AuditableEntitySaveChangesInterceptor

2. `Infrastructure/Persistence/Interceptors/AuditableEntitySaveChangesInterceptor.cs`
   - Auto-set CreatedAt on insert, UpdatedAt on update
   - Auto-set DeletedAt when IsDeleted changes to true

3. Create EF entity configurations in `Infrastructure/Persistence/Configurations/` for ALL entities:
   - UserConfiguration.cs
   - UserExternalLoginConfiguration.cs
   - RefreshTokenConfiguration.cs
   - UserProfileConfiguration.cs
   - UserSettingsConfiguration.cs
   - UserStreakConfiguration.cs
   - LearningPathConfiguration.cs
   - LearningPathUnitConfiguration.cs
   - LessonConfiguration.cs
   - UnitLessonConfiguration.cs
   - LessonCompletionConfiguration.cs
   - FlashcardDeckConfiguration.cs
   - FlashcardConfiguration.cs
   - FlashcardReviewConfiguration.cs
   - PronunciationSessionConfiguration.cs
   - PronunciationAttemptConfiguration.cs
   - ConversationSessionConfiguration.cs
   - ConversationMessageConfiguration.cs
   - RoleplayScenarioConfiguration.cs
   - WritingSubmissionConfiguration.cs
   - WritingFeedbackConfiguration.cs
   - ReadingArticleConfiguration.cs
   - ReadingSessionConfiguration.cs
   - ListeningExerciseConfiguration.cs
   - DictationAttemptConfiguration.cs
   - GrammarTopicConfiguration.cs
   - GrammarQuestionConfiguration.cs
   - GrammarQuizAttemptConfiguration.cs
   - XpTransactionConfiguration.cs
   - AchievementConfiguration.cs
   - UserAchievementConfiguration.cs
   - LeaderboardEntryConfiguration.cs
   - VocabularyBankConfiguration.cs
   - DailyChallengeConfiguration.cs
   - DailyChallengeCompletionConfiguration.cs

Each configuration must:
- Set table name (snake_case)
- Set primary key
- Configure max lengths for strings
- Set required/optional
- Configure relationships with correct delete behavior
- Add HasQueryFilter for soft-deletable entities
- Add indexes for foreign keys and common queries
- Use .HasColumnType("jsonb") for JSONB columns
- Set default values where needed

4. `Infrastructure/DependencyInjection.cs`
   - Register AppDbContext with PostgreSQL
   - Register all infrastructure services
   - Register Redis

Reference `docs/database/DATABASE_SCHEMA.md` for exact column specs.
```

---

## 🔧 PROMPT 0.7 — Infrastructure Services + API Setup

```
Create the remaining Infrastructure services and the API project setup.

**Infrastructure/Services/Auth/:**
- `JwtService.cs` — generate access token with claims (sub, email, name, role, level), validate token, generate refresh token string
- `CurrentUserService.cs` — implement ICurrentUserService, extract from HttpContext.User claims

**Infrastructure/Caching/:**
- `RedisCacheService.cs` — implement ICacheService using StackExchange.Redis, handle serialization with System.Text.Json

**Infrastructure/Services/Storage/:**
- `MinioStorageService.cs` — implement IFileStorageService using MinIO SDK

**API Setup:**

1. `API/Program.cs` — configure ALL services:
   - Serilog logging
   - Application DI
   - Infrastructure DI
   - CORS (allow frontend origins)
   - JWT authentication
   - Authorization
   - SignalR
   - Rate limiting
   - Swagger/OpenAPI
   - Health checks (postgres, redis)
   - Global exception middleware
   - Map controllers, hubs, health checks

2. `API/Middleware/GlobalExceptionMiddleware.cs`
   - Catch all exceptions, map to proper HTTP status codes and ApiErrorResponse

3. `API/Filters/ApiExceptionFilterAttribute.cs`
   - Alternative exception handling for controllers

4. Create docker-compose.yml in `docker/` with postgres, redis, seq, minio services
   (reference docs/devops/DEVOPS.md)

5. Create `.env.example` with all environment variables

6. Create initial EF migration:
   Run: `dotnet ef migrations add InitialCreate -p src/EnglishAI.Infrastructure -s src/EnglishAI.API`
```

---

## 🔧 PROMPT 1.1 — Auth Feature (Commands + Queries)

```
Implement the complete Authentication feature for EnglishAI using CQRS pattern.

Create in `Application/Features/Auth/`:

**Commands:**
1. `Register/RegisterCommand.cs` + Handler + Validator
   - Input: email, password, displayName
   - Create User + UserProfile + UserSettings + UserStreak
   - Hash password with BCrypt
   - Generate JWT + refresh token
   - Return: UserDto + accessToken

2. `Login/LoginCommand.cs` + Handler + Validator
   - Validate email + password
   - Update lastLoginAt
   - Generate JWT + refresh token
   - Return: UserDto + accessToken

3. `GoogleLogin/GoogleLoginCommand.cs` + Handler
   - Input: Google idToken
   - Verify with Google, create or link user
   - Return: UserDto + accessToken + isNewUser flag

4. `RefreshToken/RefreshTokenCommand.cs` + Handler
   - Input: refreshToken from cookie
   - Validate, rotate (revoke old, issue new)
   - Return: new accessToken

5. `Logout/LogoutCommand.cs` + Handler
   - Revoke refresh token

6. `ForgotPassword/ForgotPasswordCommand.cs` + Handler
   - Generate reset token, store in DB, send email (log for now)

7. `ResetPassword/ResetPasswordCommand.cs` + Handler
   - Validate token, update password hash

**Queries:**
8. `GetCurrentUser/GetCurrentUserQuery.cs` + Handler
   - Return full user with profile + settings + streak

**DTOs:**
- UserDto, AuthResponseDto, UserProfileDto, UserSettingsDto

**API Controller:**
- `AuthController.cs` with all endpoints matching docs/api/API_SPEC.md
- Set refresh token as httpOnly cookie on login/register
- Read refresh token from cookie on refresh

Include proper FluentValidation on all commands.
```

---

## 🔧 PROMPT 2.1 — Dashboard + Onboarding

```
Implement Dashboard and Onboarding features for EnglishAI.

**Onboarding (Application/Features/Onboarding/):**

1. `UpdateProfile/UpdateOnboardingProfileCommand.cs` + Handler + Validator
   - Update UserProfile: nativeLanguage, currentLevel, targetLevel, dailyGoalMinutes, learningPurpose, interests

2. `PlacementTest/SubmitPlacementTestCommand.cs` + Handler
   - Input: array of question answers
   - Calculate recommended CEFR level based on score
   - Update UserProfile.currentLevel

3. `Complete/CompleteOnboardingCommand.cs` + Handler
   - Set onboardingCompleted = true
   - Trigger learning path generation (can be basic/placeholder for now)

**Dashboard (Application/Features/Dashboard/):**

4. `GetDashboard/GetDashboardQuery.cs` + Handler
   Implement ICacheable (5 min cache).
   Aggregate:
   - Streak info from UserStreak
   - Today's progress: count lesson completions + total time today
   - XP: total from xp_transactions, today earned
   - Due flashcard count
   - Current lesson (latest incomplete)
   - Weekly activity (last 7 days aggregated by date)
   - Recent achievements (last 3)

5. `GetActivity/GetActivityHistoryQuery.cs` + Handler
   - Input: days (7, 30, 90)
   - Return daily breakdown of activities

**API Controllers:**
- `OnboardingController.cs`
- `DashboardController.cs`

All endpoints match docs/api/API_SPEC.md.
```

---

## 🔧 PROMPT 3.1 — Flashcard + SRS Feature

```
Implement the complete Flashcard + SRS feature for EnglishAI based on docs/features/SRS_ALGORITHM.md.

**Domain Value Object:**
Create `Domain/ValueObjects/SrsData.cs` with the SM-2 algorithm:
- EaseFactor, IntervalDays, Repetitions, NextReviewAt, LastReviewedAt
- Review(quality) method implementing SM-2 formula
- CreateNew() static factory

**Application/Features/Flashcards/:**

Commands:
1. `CreateDeck/CreateDeckCommand.cs` + Handler + Validator
2. `CreateCard/CreateCardCommand.cs` + Handler + Validator
3. `BulkCreateCards/BulkCreateCardsCommand.cs` + Handler + Validator
4. `ReviewCard/ReviewCardCommand.cs` + Handler + Validator
   - Apply SM-2 algorithm, update card SRS fields
   - Create FlashcardReview log entry
   - Award XP (every 10 reviews = 5 XP)
5. `DeleteDeck/DeleteDeckCommand.cs` + Handler (soft delete)
6. `GenerateFlashcards/GenerateFlashcardsCommand.cs` + Handler
   - Use IAiCompletionService to generate cards from a topic
   - Input: topic, count, cefrLevel

Queries:
7. `GetDecks/GetDecksQuery.cs` + Handler (paginated, with due count per deck)
8. `GetCards/GetCardsQuery.cs` + Handler (filter: all/due/new)
9. `GetDueCards/GetDueCardsQuery.cs` + Handler (ordered by overdue priority)
10. `GetSrsStats/GetSrsStatsQuery.cs` + Handler (cards breakdown + retention rate)

DTOs: FlashcardDeckDto, FlashcardDto, SrsStatsDto, ReviewResultDto

API: `FlashcardsController.cs` + `FlashcardDecksController.cs`

Write unit tests for the SM-2 algorithm in `tests/EnglishAI.UnitTests/Domain/SrsDataTests.cs`:
- Test quality 0 (reset)
- Test quality 3 (correct, first time)
- Test quality 4 (correct, subsequent)
- Test quality 5 (easy)
- Test ease factor minimum bound (1.3)
- Test interval progression: 1 → 6 → 6*EF → ...
```

---

## 🔧 PROMPT 4.1 — Pronunciation AI Feature

```
Implement the Pronunciation AI feature — the #1 priority feature of EnglishAI.
Reference: docs/features/PRONUNCIATION_AI.md

**Application/Features/Pronunciation/:**

Commands:
1. `CreateSession/CreatePronunciationSessionCommand.cs` + Handler + Validator
   - Create session with targetText and targetPhonemes
   - Generate reference audio via ISpeechService.SynthesizeAsync
   - Return sessionId + targetPhonemes + referenceAudioUrl

2. `Evaluate/EvaluatePronunciationCommand.cs` + Handler
   - Accept audio stream (from multipart upload)
   - Process audio: validate format, convert if needed
   - Call IPronunciationAssessmentService.EvaluateAsync
   - Generate AI feedback via IAiCompletionService
   - Extract waveform data
   - Store audio file via IFileStorageService
   - Save PronunciationAttempt to DB
   - Award XP (5 per attempt)
   - Return full scoring + phoneme detail + feedback + waveform

Queries:
3. `GetSessionAttempts/GetSessionAttemptsQuery.cs` + Handler
4. `GetHistory/GetPronunciationHistoryQuery.cs` + Handler (with stats)
5. `GetMinimalPairs/GetMinimalPairsQuery.cs` + Handler

**Infrastructure/Services/Speech/:**
6. `AzureSpeechService.cs` — implement IPronunciationAssessmentService
   - Use Azure Pronunciation Assessment API with Phoneme granularity
   - Map results to our domain models

7. `WhisperService.cs` — fallback STT service

8. `AudioProcessingService.cs` — IAudioProcessingService
   - ConvertToWavAsync
   - ExtractWaveformData (200 sample points)
   - GetAudioDuration
   - ValidateAudioFile

**Infrastructure/Services/AI/:**
9. `PronunciationFeedbackService.cs`
   - Generate personalized feedback considering Vietnamese learner common mistakes
   - Focus on weak phonemes, mouth positioning tips

**API:**
10. `PronunciationController.cs` with multipart file upload
11. `PronunciationHub.cs` (SignalR) for real-time audio streaming
    - StartSession, SendAudioChunk, EndSession
    - Server sends: VolumeLevel, PartialResult, FinalResult

**DTOs:**
PronunciationSessionDto, PronunciationAttemptDto, PronunciationScoresDto, PhonemeScoreDto, WordScoreDto, WaveformDataDto, MinimalPairDto

Create models that match the API response format in docs/api/API_SPEC.md section 7.
```

---

## 🔧 PROMPT 5.1 — Grammar Feature

```
Implement Grammar lessons and quiz feature + AI grammar checker for EnglishAI.

**Application/Features/Grammar/:**

Commands:
1. `SubmitQuiz/SubmitGrammarQuizCommand.cs` + Handler + Validator
   - Grade answers, calculate score
   - Save GrammarQuizAttempt
   - Award XP based on score
2. `CheckGrammar/CheckGrammarCommand.cs` + Handler
   - Send text to IAiCompletionService with grammar checker prompt from docs/features/AI_INTEGRATION.md
   - Parse JSON response
   - Return corrections with explanations

Queries:
3. `GetTopics/GetGrammarTopicsQuery.cs` + Handler (filter by level + category)
4. `GetTopicDetail/GetTopicDetailQuery.cs` + Handler (by slug)
5. `GetQuiz/GetGrammarQuizQuery.cs` + Handler (random N questions from topic)

DTOs: GrammarTopicDto, GrammarQuestionDto, QuizResultDto, GrammarCorrectionDto

API: `GrammarController.cs`

Also create seed data file `Infrastructure/Persistence/Seeds/GrammarSeedData.cs`:
- 10 grammar topics across A1-B2
- 5 questions each (mix of question types)
- Seed via HasData in migration or via IHostedService on startup
```

---

## 🔧 PROMPT 6.1 — Gamification (XP + Achievements + Streaks)

```
Implement the Gamification system for EnglishAI.

**Application/Features/Gamification/:**

Commands:
1. `AwardXp/AwardXpCommand.cs` + Handler
   - Create XpTransaction, update balance
   - Check for level up
   - Publish domain event: XpAwardedEvent

2. `CheckAchievements/CheckAchievementsCommand.cs` + Handler
   - Evaluate all locked achievements against user's current stats
   - Unlock matching achievements
   - Award bonus XP for each
   - Send SignalR notification

3. `UpdateStreak/UpdateStreakCommand.cs` + Handler
   - Called when user completes any activity
   - If lastActivityDate != today: increment streak
   - If gap > 1 day && freezeCount > 0: use freeze
   - If gap > 1 day && no freeze: reset streak
   - Update longestStreak if current > longest

Queries:
4. `GetAchievements/GetAchievementsQuery.cs` + Handler (unlocked + locked)
5. `GetLeaderboard/GetLeaderboardQuery.cs` + Handler (weekly/monthly/all_time, paginated)
6. `GetXpHistory/GetXpHistoryQuery.cs` + Handler

**Domain Events:**
- `LessonCompletedEvent` → triggers AwardXp + CheckAchievements + UpdateStreak
- `FlashcardReviewedEvent` → triggers AwardXp
- `PronunciationAttemptedEvent` → triggers AwardXp + CheckAchievements

**Seed Data:**
Create 30+ achievements:
- Streak: 3-day, 7-day, 14-day, 30-day, 100-day
- Lessons: First lesson, 10 lessons, 50 lessons, 100 lessons
- Pronunciation: First recording, Score 80+, Score 95+, 100 attempts
- Vocabulary: 50 words, 200 words, 500 words, 1000 words
- Writing: First submission, 10 submissions
- Conversation: First chat, 1 hour total chat time
- Misc: Night owl (learn after 11pm), Early bird (before 7am)

API: `GamificationController.cs`, `LeaderboardController.cs`
SignalR: `NotificationHub.cs` — push achievement unlocks, XP updates, streak reminders
```

---

## 🔧 PROMPT 7.1 — AI Conversation + Writing + Reading

```
Implement remaining AI features for EnglishAI: Conversation, Writing, Reading, Listening.

**Conversation (Application/Features/Conversations/):**
1. `StartSession/StartConversationCommand.cs` + Handler
   - Create session with system prompt based on type (free_chat/roleplay)
   - For roleplay: load RoleplayScenario, use its systemPrompt
   - Return starterMessage
2. `SendMessage/SendMessageCommand.cs` + Handler
   - Add user message to session
   - Call IAiCompletionService for AI response
   - Detect grammar issues in user message (separate AI call or inline)
   - Save both messages
3. `EndSession/EndConversationCommand.cs` + Handler
   - Generate session feedback (grammar score, vocab score, fluency, tips)
   - Save feedback to session
4. `GetScenarios/GetScenariosQuery.cs` + Handler
5. API: `ConversationsController.cs`
6. SignalR: `ConversationHub.cs` — streaming AI responses

**Writing (Application/Features/Writing/):**
7. `GetPrompts/GetWritingPromptsQuery.cs` + Handler
8. `Submit/SubmitWritingCommand.cs` + Handler
   - Call IAiCompletionService with writing feedback prompt from docs/features/AI_INTEGRATION.md
   - Parse structured response (scores + corrections + tips)
   - Save submission + feedback
9. API: `WritingController.cs`

**Reading (Application/Features/Reading/):**
10. `GetArticles/GetArticlesQuery.cs` + Handler (paginated, filter by level + category)
11. `GetArticle/GetArticleDetailQuery.cs` + Handler
12. `SaveSession/SaveReadingSessionCommand.cs` + Handler
13. `AskAi/AskAiAboutArticleCommand.cs` + Handler
    - Context-aware Q&A about the article
14. API: `ReadingController.cs`

**Listening (Application/Features/Listening/):**
15. `GetExercises/GetListeningExercisesQuery.cs` + Handler
16. `SubmitDictation/SubmitDictationCommand.cs` + Handler
    - Compare user text with transcript, word-by-word diff
    - Calculate accuracy percentage
17. API: `ListeningController.cs`

**Infrastructure/Services/AI/:**
18. `OpenAiCompletionService.cs` — implement IAiCompletionService
    - Support both regular and streaming completions
    - Use OpenAI .NET SDK
    - Handle rate limits with Polly retry
19. `ConversationAiService.cs` — manage conversation context, system prompts
20. `WritingFeedbackService.cs` — structured writing analysis

All endpoints match docs/api/API_SPEC.md.
Seed roleplay scenarios (10+ scenarios across categories).
```

---

## 🔧 PROMPT 8.1 — Profile, Settings, Vocabulary, Admin

```
Implement remaining features: Profile, Settings, Vocabulary Bank, and Admin endpoints.

**Profile & Settings (Application/Features/Users/):**
1. `UpdateProfile/UpdateProfileCommand.cs` + Handler
2. `UpdateSettings/UpdateSettingsCommand.cs` + Handler
3. `UploadAvatar/UploadAvatarCommand.cs` + Handler (file upload to storage)
4. `DeleteAccount/DeleteAccountCommand.cs` + Handler (soft delete + schedule cleanup)
5. `GetProgress/GetProgressOverviewQuery.cs` + Handler
   - Aggregate skill breakdown scores
   - Weekly goal progress
   - XP + level info
6. `GetHistory/GetProgressHistoryQuery.cs` + Handler
7. API: `UsersController.cs`, `ProgressController.cs`

**Vocabulary Bank (Application/Features/Vocabulary/):**
8. `GetVocabulary/GetVocabularyQuery.cs` + Handler (search, filter by mastery, paginated)
9. `AddWord/AddWordCommand.cs` + Handler + Validator
10. `UpdateMastery/UpdateMasteryCommand.cs` + Handler
11. API: `VocabularyController.cs`

**Admin (Application/Features/Admin/):**
12. `GetUsers/GetAdminUsersQuery.cs` + Handler (paginated, search)
13. `GetStats/GetAdminStatsQuery.cs` + Handler (DAU, MAU, popular lessons, etc.)
14. `CreateLesson/CreateLessonCommand.cs` + Handler
15. `UpdateLesson/UpdateLessonCommand.cs` + Handler
16. `CreateArticle/CreateArticleCommand.cs` + Handler
17. API: `AdminController.cs` with `[Authorize(Roles = "Admin")]`
```

---

## 🔧 PROMPT 9.1 — Tests + CI/CD + Final Polish

```
Add comprehensive testing and CI/CD for EnglishAI.

**Architecture Tests (ArchTests/):**
1. Domain has no dependencies on Application/Infrastructure/API
2. Application depends only on Domain
3. Infrastructure does not depend on API
4. All handlers are internal/sealed
5. All commands have validators
6. All entities inherit from BaseEntity

**Unit Tests:**
7. SrsData SM-2 algorithm (all edge cases)
8. Auth handlers (register, login)
9. Flashcard review handler
10. Achievement checking logic
11. Streak update logic
12. Dictation accuracy calculator
13. XP award logic

**Integration Tests:**
14. Auth flow: register → login → refresh → me → logout
15. Flashcard CRUD + review flow
16. Pronunciation evaluation (mock Azure Speech)
17. Dashboard aggregation

Use Testcontainers for PostgreSQL in integration tests.
Use Bogus for test data generation.

**CI/CD:**
18. Create `.github/workflows/ci.yml` matching docs/devops/DEVOPS.md
19. Create `.github/workflows/cd.yml` for deployment

**Final Polish:**
20. Add Swagger XML documentation on all controllers
21. Add response type attributes ([ProducesResponseType])
22. Add API versioning
23. Add response compression
24. Review and fix any N+1 query issues
25. Add missing indexes
26. Create final README.md with badges, screenshots placeholder, setup instructions
```

---

## 💡 Tips khi dùng Cursor

1. **Paste `.cursorrules` file vào project root** — Cursor sẽ tự động đọc làm context
2. **Mỗi prompt chỉ làm 1 sprint** — đừng paste hết cùng lúc
3. **Sau mỗi prompt, kiểm tra code build được** — `dotnet build`
4. **Nếu Cursor bỏ sót file nào**, chỉ cần nhắc: "You missed creating X, please create it"
5. **Khi gặp lỗi compile**, paste error vào Cursor và nói "Fix this"
6. **Sau Sprint 0**, chạy `dotnet ef migrations add InitialCreate` để tạo migration
7. **Reference docs**: Khi Cursor hỏi về schema → chỉ nó đọc `docs/database/DATABASE_SCHEMA.md`

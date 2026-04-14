# TODO Plan — EnglishAI

## Development Strategy
- **Sprint length**: 1 week
- **Total estimated**: 10-12 sprints (backend only)
- **Approach**: Feature-complete backend → then frontend

---

## Sprint 0: Project Setup (Day 1-2)

### Solution Structure
- [ ] Create solution `EnglishAI.sln`
- [ ] Create `EnglishAI.Domain` class library
- [ ] Create `EnglishAI.Application` class library
- [ ] Create `EnglishAI.Infrastructure` class library
- [ ] Create `EnglishAI.Shared` class library
- [ ] Create `EnglishAI.API` web project
- [ ] Create `EnglishAI.UnitTests` test project
- [ ] Create `EnglishAI.IntegrationTests` test project
- [ ] Create `EnglishAI.ArchTests` test project
- [ ] Set up project references (enforce dependency rule)

### NuGet Packages
- [ ] Domain: (none — keep it clean)
- [ ] Application: MediatR, FluentValidation, AutoMapper
- [ ] Infrastructure: EF Core + Npgsql, StackExchange.Redis, Microsoft.CognitiveServices.Speech, OpenAI SDK, Serilog, MinIO SDK
- [ ] API: Swashbuckle/Scalar, Serilog.AspNetCore, AspNetCore.Authentication.JwtBearer, AspNetCore.Authentication.Google, AspNetCore.SignalR
- [ ] Tests: xUnit, FluentAssertions, Moq, Testcontainers, Bogus

### Infrastructure
- [ ] docker-compose.yml (postgres, redis, seq, minio)
- [ ] .env.example
- [ ] .editorconfig
- [ ] .gitignore
- [ ] Directory.Build.props (shared settings)
- [ ] GitHub Actions CI workflow

### Base Code
- [ ] Domain/Common/BaseEntity.cs (Id, CreatedAt, UpdatedAt)
- [ ] Domain/Common/AuditableEntity.cs
- [ ] Domain/Common/ValueObject.cs
- [ ] Application DI registration
- [ ] Infrastructure DI registration
- [ ] API Program.cs with all middleware
- [ ] Global exception middleware
- [ ] Serilog configuration
- [ ] Swagger/Scalar configuration
- [ ] CORS configuration
- [ ] Health checks

---

## Sprint 1: Auth + User Management

### Domain
- [ ] `User` entity
- [ ] `UserProfile` entity
- [ ] `UserSettings` entity
- [ ] `UserStreak` entity
- [ ] `RefreshToken` entity
- [ ] `UserExternalLogin` entity
- [ ] Role enum (Learner, Premium, Admin)
- [ ] CefrLevel enum (A1-C2)

### Infrastructure — Persistence
- [ ] `AppDbContext` with all DbSets
- [ ] Entity configurations for all auth entities
- [ ] Initial migration
- [ ] `ICurrentUserService` implementation (extract from JWT)
- [ ] `AuditableEntitySaveChangesInterceptor`

### Application — Auth Features
- [ ] `RegisterCommand` + Handler + Validator
- [ ] `LoginCommand` + Handler + Validator
- [ ] `GoogleLoginCommand` + Handler
- [ ] `RefreshTokenCommand` + Handler
- [ ] `LogoutCommand` + Handler
- [ ] `ForgotPasswordCommand` + Handler
- [ ] `ResetPasswordCommand` + Handler
- [ ] `GetCurrentUserQuery` + Handler

### Infrastructure — Services
- [ ] `JwtService` (generate + validate tokens)
- [ ] `PasswordHasher` (BCrypt)
- [ ] `GoogleAuthService`

### API
- [ ] `AuthController` with all endpoints
- [ ] JWT authentication configuration
- [ ] Refresh token cookie handling
- [ ] Rate limiting on auth endpoints

### Tests
- [ ] Unit tests for JwtService
- [ ] Unit tests for RegisterCommand handler
- [ ] Unit tests for LoginCommand handler
- [ ] Integration test: register → login → refresh → logout flow

---

## Sprint 2: Onboarding + Dashboard

### Application — Onboarding
- [ ] `UpdateOnboardingProfileCommand` + Handler
- [ ] `SubmitPlacementTestCommand` + Handler
- [ ] `CompleteOnboardingCommand` + Handler (generates learning path)

### Application — Dashboard
- [ ] `GetDashboardQuery` + Handler
- [ ] `GetActivityHistoryQuery` + Handler

### Infrastructure
- [ ] Redis caching service
- [ ] `CachingBehavior` pipeline behavior
- [ ] Dashboard data aggregation queries

### Tests
- [ ] Unit tests for onboarding commands
- [ ] Unit tests for dashboard query

---

## Sprint 3: Learning Path + Lessons

### Domain
- [ ] `LearningPath` entity
- [ ] `LearningPathUnit` entity
- [ ] `Lesson` entity
- [ ] `UnitLesson` entity
- [ ] `LessonCompletion` entity
- [ ] Lesson content JSON schema definition

### Application
- [ ] `GetLearningPathsQuery` + Handler
- [ ] `GetLearningPathDetailQuery` + Handler
- [ ] `GenerateLearningPathCommand` + Handler (AI-powered)
- [ ] `GetLessonQuery` + Handler
- [ ] `StartLessonCommand` + Handler
- [ ] `CompleteLessonCommand` + Handler
- [ ] `RegenerateLearningPathCommand` + Handler

### Infrastructure
- [ ] EF configurations for all learning entities
- [ ] Migration
- [ ] Learning path generation algorithm
- [ ] Seed data: sample lessons per level

### Tests
- [ ] Unit tests for path generation
- [ ] Unit tests for lesson completion + XP award

---

## Sprint 4: Flashcard + SRS

### Domain
- [ ] `FlashcardDeck` entity
- [ ] `Flashcard` entity
- [ ] `FlashcardReview` entity
- [ ] `SrsData` value object (SM-2)

### Application
- [ ] `GetDecksQuery` + Handler
- [ ] `CreateDeckCommand` + Handler + Validator
- [ ] `GetCardsQuery` + Handler (with due filter)
- [ ] `CreateCardCommand` + Handler + Validator
- [ ] `BulkCreateCardsCommand` + Handler
- [ ] `ReviewCardCommand` + Handler (SM-2 update)
- [ ] `GetDueCardsQuery` + Handler
- [ ] `GetSrsStatsQuery` + Handler
- [ ] `GenerateFlashcardsCommand` + Handler (AI)
- [ ] `DeleteDeckCommand` + Handler (soft delete)

### Tests
- [ ] Unit tests for SM-2 algorithm (all quality scenarios)
- [ ] Unit tests for review scheduling
- [ ] Integration tests for review flow

---

## Sprint 5: Grammar

### Domain
- [ ] `GrammarTopic` entity
- [ ] `GrammarQuestion` entity
- [ ] `GrammarQuizAttempt` entity

### Application
- [ ] `GetGrammarTopicsQuery` + Handler
- [ ] `GetGrammarTopicDetailQuery` + Handler
- [ ] `GetGrammarQuizQuery` + Handler (random questions)
- [ ] `SubmitGrammarQuizCommand` + Handler
- [ ] `CheckGrammarCommand` + Handler (AI grammar checker)

### Infrastructure
- [ ] Seed data: 50+ grammar topics with questions
- [ ] AI grammar checking service

### Tests
- [ ] Unit tests for quiz scoring
- [ ] Unit tests for grammar checking

---

## Sprint 6: Pronunciation AI ⭐

### Domain
- [ ] `PronunciationSession` entity
- [ ] `PronunciationAttempt` entity

### Application
- [ ] `CreatePronunciationSessionCommand` + Handler
- [ ] `EvaluatePronunciationCommand` + Handler
- [ ] `GetPronunciationHistoryQuery` + Handler
- [ ] `GetPronunciationStatsQuery` + Handler
- [ ] `GetMinimalPairsQuery` + Handler

### Infrastructure
- [ ] `AzureSpeechService` (pronunciation assessment)
- [ ] `WhisperService` (STT fallback)
- [ ] `AudioProcessingService` (WAV conversion, waveform)
- [ ] `PronunciationFeedbackService` (AI tips)
- [ ] Audio file storage (MinIO/Blob)
- [ ] TTS reference audio generation + caching

### API
- [ ] `PronunciationController`
- [ ] `PronunciationHub` (SignalR)
- [ ] Audio upload handling (multipart)
- [ ] Rate limiting for pronunciation

### Tests
- [ ] Unit tests for audio processing
- [ ] Unit tests for waveform extraction
- [ ] Unit tests for score aggregation
- [ ] Integration test: upload audio → get scores

---

## Sprint 7: XP + Achievements + Gamification

### Domain
- [ ] `XpTransaction` entity
- [ ] `Achievement` entity
- [ ] `UserAchievement` entity
- [ ] `LeaderboardEntry` entity
- [ ] `DailyChallenge` entity

### Application
- [ ] `AwardXpCommand` + Handler
- [ ] `CheckAchievementsCommand` + Handler
- [ ] `GetAchievementsQuery` + Handler
- [ ] `GetLeaderboardQuery` + Handler
- [ ] `GetXpHistoryQuery` + Handler
- [ ] `UpdateStreakCommand` + Handler
- [ ] `GetDailyChallengeQuery` + Handler

### Infrastructure
- [ ] Achievement checking engine (evaluate conditions)
- [ ] Leaderboard refresh background job
- [ ] Daily challenge generation job
- [ ] SignalR notification hub

### Seed Data
- [ ] 30+ achievements with conditions
- [ ] XP reward configuration

### Tests
- [ ] Unit tests for achievement checking
- [ ] Unit tests for streak logic
- [ ] Unit tests for XP calculation

---

## Sprint 8: Reading + Listening

### Domain
- [ ] `ReadingArticle` entity
- [ ] `ReadingSession` entity
- [ ] `ListeningExercise` entity
- [ ] `DictationAttempt` entity

### Application
- [ ] `GetArticlesQuery` + Handler
- [ ] `GetArticleDetailQuery` + Handler
- [ ] `SaveReadingSessionCommand` + Handler
- [ ] `AskAiAboutArticleCommand` + Handler
- [ ] `GetListeningExercisesQuery` + Handler
- [ ] `SubmitDictationCommand` + Handler

### Infrastructure
- [ ] AI reading assistant service
- [ ] Dictation accuracy comparison algorithm
- [ ] Seed data: sample articles and exercises

---

## Sprint 9: Writing + AI Conversation

### Domain
- [ ] `WritingSubmission` entity
- [ ] `WritingFeedback` entity
- [ ] `ConversationSession` entity
- [ ] `ConversationMessage` entity
- [ ] `RoleplayScenario` entity

### Application
- [ ] `GetWritingPromptsQuery` + Handler
- [ ] `SubmitWritingCommand` + Handler (triggers AI feedback)
- [ ] `StartConversationCommand` + Handler
- [ ] `SendMessageCommand` + Handler
- [ ] `SendVoiceMessageCommand` + Handler
- [ ] `EndConversationCommand` + Handler (generates feedback)
- [ ] `GetScenariosQuery` + Handler

### Infrastructure
- [ ] Writing feedback AI service
- [ ] Conversation AI service (streaming)
- [ ] `ConversationHub` (SignalR streaming)
- [ ] Seed data: roleplay scenarios

---

## Sprint 10: Profile, Settings, Vocabulary

### Application
- [ ] `UpdateProfileCommand` + Handler
- [ ] `UpdateSettingsCommand` + Handler
- [ ] `UploadAvatarCommand` + Handler
- [ ] `DeleteAccountCommand` + Handler
- [ ] `GetVocabularyQuery` + Handler
- [ ] `AddVocabularyCommand` + Handler
- [ ] `UpdateMasteryCommand` + Handler

---

## Sprint 11: Admin + Polish

### Application
- [ ] Admin CRUD for lessons
- [ ] Admin CRUD for articles
- [ ] Admin CRUD for exercises
- [ ] Admin dashboard stats
- [ ] `GetAdminStatsQuery` + Handler

### Cross-cutting
- [ ] API versioning
- [ ] Response compression
- [ ] Request/response logging
- [ ] Performance optimization (N+1 queries, projection)
- [ ] Security audit (OWASP top 10)

---

## Sprint 12: Testing + Documentation + Deploy

- [ ] Increase unit test coverage to 80%+
- [ ] Add missing integration tests
- [ ] Architecture tests (dependency rules)
- [ ] API documentation (Swagger annotations)
- [ ] README finalization
- [ ] Docker production build optimization
- [ ] Deploy to Railway/Azure
- [ ] Set up monitoring (health checks + Seq)
- [ ] Performance testing with k6 or wrk
- [ ] Create demo video for portfolio

---

## Post-Backend: Frontend Sprints
(Separate planning document)
- Sprint F1: Next.js setup + Auth pages
- Sprint F2: Onboarding + Dashboard
- Sprint F3: Learning path + Lessons
- Sprint F4: Flashcard UI + SRS
- Sprint F5: Pronunciation UI + Waveform
- Sprint F6: AI Conversation + Chat UI
- Sprint F7: Writing + Reading + Listening
- Sprint F8: Profile + Gamification + Polish

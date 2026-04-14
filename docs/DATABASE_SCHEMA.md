# Database Schema — EnglishAI

## Overview
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core 8
- **Naming**: snake_case (PostgreSQL convention via EF Core NamingConventions package)
- **Soft Delete**: All user-content entities have `is_deleted` + `deleted_at`
- **Audit**: All entities inherit `created_at`, `updated_at`

---

## ER Diagram (Simplified)

```
users ─────────────┬──── user_profiles
  │                │──── user_settings
  │                │──── user_streaks
  │                └──── user_achievements
  │
  ├── learning_paths ──── learning_path_units ──── unit_lessons
  │
  ├── flashcard_decks ──── flashcards ──── flashcard_reviews
  │
  ├── pronunciation_sessions ──── pronunciation_attempts
  │
  ├── conversation_sessions ──── conversation_messages
  │
  ├── writing_submissions ──── writing_feedback
  │
  ├── reading_sessions
  │
  ├── listening_sessions ──── dictation_attempts
  │
  ├── grammar_quiz_attempts ──── grammar_quiz_answers
  │
  └── xp_transactions
       achievements
       leaderboard_entries
```

---

## Table Definitions

### 1. users
Core user authentication table.
```sql
CREATE TABLE users (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email               VARCHAR(255) NOT NULL UNIQUE,
    normalized_email    VARCHAR(255) NOT NULL UNIQUE,
    password_hash       VARCHAR(500),          -- null for OAuth users
    display_name        VARCHAR(100) NOT NULL,
    avatar_url          VARCHAR(500),
    role                VARCHAR(20) NOT NULL DEFAULT 'Learner',  -- Learner, Premium, Admin
    email_confirmed     BOOLEAN NOT NULL DEFAULT FALSE,
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    last_login_at       TIMESTAMPTZ,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_users_email ON users(normalized_email);
```

### 2. user_external_logins
OAuth providers (Google, GitHub).
```sql
CREATE TABLE user_external_logins (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    provider        VARCHAR(50) NOT NULL,   -- Google, GitHub
    provider_key    VARCHAR(500) NOT NULL,
    provider_email  VARCHAR(255),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(provider, provider_key)
);
```

### 3. refresh_tokens
```sql
CREATE TABLE refresh_tokens (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token           VARCHAR(500) NOT NULL UNIQUE,
    expires_at      TIMESTAMPTZ NOT NULL,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    revoked_at      TIMESTAMPTZ,
    replaced_by     UUID REFERENCES refresh_tokens(id),
    created_by_ip   VARCHAR(45)
);

CREATE INDEX idx_refresh_tokens_user ON refresh_tokens(user_id);
CREATE INDEX idx_refresh_tokens_token ON refresh_tokens(token);
```

### 4. user_profiles
Onboarding data + learning preferences.
```sql
CREATE TABLE user_profiles (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    native_language     VARCHAR(10) NOT NULL DEFAULT 'vi',     -- ISO 639-1
    current_level       VARCHAR(5) NOT NULL DEFAULT 'A1',      -- CEFR: A1-C2
    target_level        VARCHAR(5) NOT NULL DEFAULT 'B2',
    daily_goal_minutes  INT NOT NULL DEFAULT 15,               -- 5, 10, 15, 30, 60
    learning_purpose    VARCHAR(50),                           -- exam, travel, work, hobby
    weak_skills         JSONB DEFAULT '[]',                    -- ["pronunciation","grammar"]
    interests           JSONB DEFAULT '[]',                    -- ["technology","travel","food"]
    onboarding_completed BOOLEAN NOT NULL DEFAULT FALSE,
    timezone            VARCHAR(50) DEFAULT 'Asia/Ho_Chi_Minh',
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 5. user_settings
```sql
CREATE TABLE user_settings (
    id                      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id                 UUID NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    notification_enabled    BOOLEAN NOT NULL DEFAULT TRUE,
    daily_reminder_time     TIME DEFAULT '20:00',
    sound_enabled           BOOLEAN NOT NULL DEFAULT TRUE,
    dark_mode               BOOLEAN NOT NULL DEFAULT FALSE,
    language_ui             VARCHAR(10) NOT NULL DEFAULT 'vi',  -- vi, en
    auto_play_audio         BOOLEAN NOT NULL DEFAULT TRUE,
    show_phonetic           BOOLEAN NOT NULL DEFAULT TRUE,
    created_at              TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at              TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 6. user_streaks
```sql
CREATE TABLE user_streaks (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    current_streak      INT NOT NULL DEFAULT 0,
    longest_streak      INT NOT NULL DEFAULT 0,
    last_activity_date  DATE,
    freeze_count        INT NOT NULL DEFAULT 0,        -- streak freezes available
    total_days_learned  INT NOT NULL DEFAULT 0,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

---

### 7. learning_paths
Auto-generated based on user level + goals.
```sql
CREATE TABLE learning_paths (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    title           VARCHAR(200) NOT NULL,
    description     TEXT,
    cefr_level      VARCHAR(5) NOT NULL,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    progress_pct    DECIMAL(5,2) NOT NULL DEFAULT 0,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_learning_paths_user ON learning_paths(user_id);
```

### 8. learning_path_units
```sql
CREATE TABLE learning_path_units (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    learning_path_id UUID NOT NULL REFERENCES learning_paths(id) ON DELETE CASCADE,
    title           VARCHAR(200) NOT NULL,
    description     TEXT,
    sort_order      INT NOT NULL,
    is_locked       BOOLEAN NOT NULL DEFAULT TRUE,
    is_completed    BOOLEAN NOT NULL DEFAULT FALSE,
    unit_type       VARCHAR(30) NOT NULL,   -- grammar, vocabulary, pronunciation, reading, listening, writing, mixed
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_units_path ON learning_path_units(learning_path_id);
```

### 9. lessons
Reusable lesson content (admin-created or AI-generated).
```sql
CREATE TABLE lessons (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title           VARCHAR(300) NOT NULL,
    description     TEXT,
    lesson_type     VARCHAR(30) NOT NULL,   -- grammar, vocabulary, pronunciation, reading, listening, writing
    cefr_level      VARCHAR(5) NOT NULL,
    content_json    JSONB NOT NULL,          -- structured lesson content
    duration_min    INT NOT NULL DEFAULT 10,
    xp_reward       INT NOT NULL DEFAULT 10,
    difficulty      INT NOT NULL DEFAULT 1,  -- 1-5
    tags            JSONB DEFAULT '[]',
    is_published    BOOLEAN NOT NULL DEFAULT FALSE,
    is_ai_generated BOOLEAN NOT NULL DEFAULT FALSE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_lessons_type_level ON lessons(lesson_type, cefr_level);
```

### 10. unit_lessons (junction)
```sql
CREATE TABLE unit_lessons (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    unit_id     UUID NOT NULL REFERENCES learning_path_units(id) ON DELETE CASCADE,
    lesson_id   UUID NOT NULL REFERENCES lessons(id) ON DELETE CASCADE,
    sort_order  INT NOT NULL,
    UNIQUE(unit_id, lesson_id)
);
```

### 11. lesson_completions
```sql
CREATE TABLE lesson_completions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    lesson_id       UUID NOT NULL REFERENCES lessons(id) ON DELETE CASCADE,
    score           DECIMAL(5,2),        -- 0-100
    time_spent_sec  INT NOT NULL,
    xp_earned       INT NOT NULL DEFAULT 0,
    completed_at    TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    attempt_number  INT NOT NULL DEFAULT 1
);

CREATE INDEX idx_completions_user ON lesson_completions(user_id);
CREATE INDEX idx_completions_lesson ON lesson_completions(user_id, lesson_id);
```

---

### 12. flashcard_decks
```sql
CREATE TABLE flashcard_decks (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    title           VARCHAR(200) NOT NULL,
    description     TEXT,
    is_public       BOOLEAN NOT NULL DEFAULT FALSE,
    card_count      INT NOT NULL DEFAULT 0,
    cefr_level      VARCHAR(5),
    tags            JSONB DEFAULT '[]',
    is_deleted      BOOLEAN NOT NULL DEFAULT FALSE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_decks_user ON flashcard_decks(user_id) WHERE NOT is_deleted;
```

### 13. flashcards
```sql
CREATE TABLE flashcards (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    deck_id             UUID NOT NULL REFERENCES flashcard_decks(id) ON DELETE CASCADE,
    front               VARCHAR(500) NOT NULL,      -- word/phrase
    back                VARCHAR(500) NOT NULL,      -- definition/translation
    phonetic            VARCHAR(200),               -- IPA
    example_sentence    TEXT,
    audio_url           VARCHAR(500),
    image_url           VARCHAR(500),
    part_of_speech      VARCHAR(20),                -- noun, verb, adj, etc.
    -- SRS fields (SM-2 algorithm)
    ease_factor         DECIMAL(4,2) NOT NULL DEFAULT 2.50,
    interval_days       INT NOT NULL DEFAULT 0,
    repetitions         INT NOT NULL DEFAULT 0,
    next_review_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    last_reviewed_at    TIMESTAMPTZ,
    -- metadata
    is_deleted          BOOLEAN NOT NULL DEFAULT FALSE,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_flashcards_deck ON flashcards(deck_id) WHERE NOT is_deleted;
CREATE INDEX idx_flashcards_due ON flashcards(deck_id, next_review_at) WHERE NOT is_deleted;
```

### 14. flashcard_reviews
Log mỗi lần review.
```sql
CREATE TABLE flashcard_reviews (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    flashcard_id    UUID NOT NULL REFERENCES flashcards(id) ON DELETE CASCADE,
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    quality         INT NOT NULL,           -- 0-5 (SM-2 quality)
    time_spent_ms   INT NOT NULL,
    was_correct     BOOLEAN NOT NULL,
    previous_interval INT NOT NULL,
    new_interval    INT NOT NULL,
    reviewed_at     TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_reviews_user ON flashcard_reviews(user_id);
CREATE INDEX idx_reviews_card ON flashcard_reviews(flashcard_id);
```

---

### 15. pronunciation_sessions
```sql
CREATE TABLE pronunciation_sessions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    session_type    VARCHAR(30) NOT NULL,   -- word, sentence, minimal_pair, free_speech
    target_text     TEXT NOT NULL,           -- text user should pronounce
    target_phonemes VARCHAR(500),           -- IPA target
    cefr_level      VARCHAR(5),
    lesson_id       UUID REFERENCES lessons(id),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_pron_sessions_user ON pronunciation_sessions(user_id);
```

### 16. pronunciation_attempts
```sql
CREATE TABLE pronunciation_attempts (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id          UUID NOT NULL REFERENCES pronunciation_sessions(id) ON DELETE CASCADE,
    user_id             UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    -- Audio
    audio_url           VARCHAR(500) NOT NULL,
    audio_duration_sec  DECIMAL(6,2) NOT NULL,
    -- Recognition result
    recognized_text     TEXT,
    recognized_phonemes VARCHAR(500),
    -- Scoring
    accuracy_score      DECIMAL(5,2),       -- 0-100
    fluency_score       DECIMAL(5,2),
    completeness_score  DECIMAL(5,2),
    pronunciation_score DECIMAL(5,2),       -- overall
    -- Phoneme detail
    phoneme_scores      JSONB,              -- [{phoneme:"æ", score:85, offset:0.2}, ...]
    word_scores         JSONB,              -- [{word:"hello", score:92, error_type:null}, ...]
    -- Feedback
    feedback_text       TEXT,               -- AI-generated tip
    -- Waveform
    waveform_data       JSONB,              -- for visualization
    attempted_at        TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_pron_attempts_session ON pronunciation_attempts(session_id);
CREATE INDEX idx_pron_attempts_user ON pronunciation_attempts(user_id);
```

---

### 17. conversation_sessions
AI free conversation + roleplay.
```sql
CREATE TABLE conversation_sessions (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    session_type        VARCHAR(30) NOT NULL,   -- free_chat, roleplay
    scenario_id         UUID REFERENCES roleplay_scenarios(id),
    ai_model            VARCHAR(50) NOT NULL DEFAULT 'gpt-4o',
    system_prompt       TEXT NOT NULL,
    cefr_level          VARCHAR(5) NOT NULL,
    -- Session summary
    message_count       INT NOT NULL DEFAULT 0,
    duration_sec        INT,
    grammar_errors      INT DEFAULT 0,
    vocabulary_used     INT DEFAULT 0,
    -- Feedback
    session_feedback    JSONB,  -- {grammar_score, vocab_score, fluency_score, tips:[]}
    started_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ended_at            TIMESTAMPTZ
);

CREATE INDEX idx_conv_sessions_user ON conversation_sessions(user_id);
```

### 18. conversation_messages
```sql
CREATE TABLE conversation_messages (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id      UUID NOT NULL REFERENCES conversation_sessions(id) ON DELETE CASCADE,
    role            VARCHAR(15) NOT NULL,   -- user, assistant, system
    content         TEXT NOT NULL,
    -- AI analysis of user message
    grammar_issues  JSONB,                  -- [{error:"tense", original:"I go", corrected:"I went"}]
    vocab_level     VARCHAR(5),             -- CEFR level of user's vocabulary
    audio_url       VARCHAR(500),           -- if user sent voice
    token_count     INT,
    sort_order      INT NOT NULL,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_conv_messages_session ON conversation_messages(session_id);
```

### 19. roleplay_scenarios
```sql
CREATE TABLE roleplay_scenarios (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title           VARCHAR(200) NOT NULL,
    description     TEXT NOT NULL,
    category        VARCHAR(50) NOT NULL,   -- daily_life, travel, business, interview, academic
    cefr_level      VARCHAR(5) NOT NULL,
    system_prompt   TEXT NOT NULL,           -- AI persona + situation
    starter_message TEXT NOT NULL,           -- AI's opening line
    objectives      JSONB NOT NULL,          -- ["Order food", "Ask for recommendations"]
    vocabulary_hints JSONB DEFAULT '[]',
    is_published    BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_scenarios_level ON roleplay_scenarios(cefr_level);
```

---

### 20. writing_submissions
```sql
CREATE TABLE writing_submissions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    prompt_text     TEXT NOT NULL,
    prompt_type     VARCHAR(30) NOT NULL,   -- essay, email, letter, story, summary, free
    cefr_level      VARCHAR(5) NOT NULL,
    user_text       TEXT NOT NULL,
    word_count      INT NOT NULL,
    lesson_id       UUID REFERENCES lessons(id),
    submitted_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_writing_user ON writing_submissions(user_id);
```

### 21. writing_feedback
```sql
CREATE TABLE writing_feedback (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    submission_id       UUID NOT NULL REFERENCES writing_submissions(id) ON DELETE CASCADE,
    -- Scores
    overall_score       DECIMAL(5,2) NOT NULL,  -- 0-100
    grammar_score       DECIMAL(5,2) NOT NULL,
    vocabulary_score    DECIMAL(5,2) NOT NULL,
    coherence_score     DECIMAL(5,2) NOT NULL,
    task_achievement    DECIMAL(5,2) NOT NULL,
    -- Detailed feedback
    corrected_text      TEXT NOT NULL,           -- full corrected version
    inline_corrections  JSONB NOT NULL,          -- [{offset:10, length:5, type:"grammar", original:"...", correction:"...", explanation:"..."}]
    general_feedback    TEXT NOT NULL,            -- overall comment
    improvement_tips    JSONB NOT NULL,           -- ["tip1", "tip2"]
    vocabulary_suggestions JSONB DEFAULT '[]',
    ai_model            VARCHAR(50) NOT NULL,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

---

### 22. reading_articles
```sql
CREATE TABLE reading_articles (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title           VARCHAR(300) NOT NULL,
    content         TEXT NOT NULL,
    summary         TEXT,
    cefr_level      VARCHAR(5) NOT NULL,
    category        VARCHAR(50) NOT NULL,   -- technology, science, culture, business, health, travel
    word_count      INT NOT NULL,
    estimated_min   INT NOT NULL,
    vocabulary_list JSONB DEFAULT '[]',     -- [{word:"ubiquitous", definition:"...", phonetic:"..."}]
    comprehension_questions JSONB DEFAULT '[]',
    source_url      VARCHAR(500),
    image_url       VARCHAR(500),
    is_ai_generated BOOLEAN NOT NULL DEFAULT FALSE,
    is_published    BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_articles_level ON reading_articles(cefr_level);
```

### 23. reading_sessions
```sql
CREATE TABLE reading_sessions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    article_id      UUID NOT NULL REFERENCES reading_articles(id),
    time_spent_sec  INT NOT NULL DEFAULT 0,
    progress_pct    DECIMAL(5,2) NOT NULL DEFAULT 0,
    quiz_score      DECIMAL(5,2),
    words_looked_up JSONB DEFAULT '[]',     -- words user tapped for definition
    completed_at    TIMESTAMPTZ,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

---

### 24. listening_exercises
```sql
CREATE TABLE listening_exercises (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title           VARCHAR(300) NOT NULL,
    audio_url       VARCHAR(500) NOT NULL,
    transcript      TEXT NOT NULL,
    cefr_level      VARCHAR(5) NOT NULL,
    exercise_type   VARCHAR(30) NOT NULL,   -- dictation, fill_blank, comprehension, podcast
    duration_sec    INT NOT NULL,
    category        VARCHAR(50),
    questions       JSONB DEFAULT '[]',
    is_published    BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 25. dictation_attempts
```sql
CREATE TABLE dictation_attempts (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    exercise_id     UUID NOT NULL REFERENCES listening_exercises(id),
    user_text       TEXT NOT NULL,
    accuracy_pct    DECIMAL(5,2) NOT NULL,
    errors          JSONB NOT NULL,         -- [{position:5, expected:"their", actual:"there"}]
    time_spent_sec  INT NOT NULL,
    attempted_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

---

### 26. grammar_topics
```sql
CREATE TABLE grammar_topics (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title           VARCHAR(200) NOT NULL,
    slug            VARCHAR(200) NOT NULL UNIQUE,
    explanation     TEXT NOT NULL,           -- rich markdown
    examples        JSONB NOT NULL,          -- [{sentence:"...", translation:"...", highlight:"..."}]
    cefr_level      VARCHAR(5) NOT NULL,
    category        VARCHAR(50) NOT NULL,   -- tenses, articles, prepositions, conditionals, etc.
    sort_order      INT NOT NULL,
    is_published    BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 27. grammar_questions
```sql
CREATE TABLE grammar_questions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    topic_id        UUID NOT NULL REFERENCES grammar_topics(id) ON DELETE CASCADE,
    question_type   VARCHAR(30) NOT NULL,   -- multiple_choice, fill_blank, reorder, error_correction
    question_text   TEXT NOT NULL,
    options         JSONB,                  -- for multiple_choice
    correct_answer  TEXT NOT NULL,
    explanation     TEXT NOT NULL,
    difficulty      INT NOT NULL DEFAULT 1, -- 1-5
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 28. grammar_quiz_attempts
```sql
CREATE TABLE grammar_quiz_attempts (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    topic_id        UUID NOT NULL REFERENCES grammar_topics(id),
    score           DECIMAL(5,2) NOT NULL,
    total_questions INT NOT NULL,
    correct_count   INT NOT NULL,
    time_spent_sec  INT NOT NULL,
    answers         JSONB NOT NULL,         -- [{question_id:"...", user_answer:"...", is_correct:true}]
    attempted_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_grammar_attempts_user ON grammar_quiz_attempts(user_id);
```

---

### 29. xp_transactions
Gamification — every XP earn/spend.
```sql
CREATE TABLE xp_transactions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    amount          INT NOT NULL,           -- positive = earn, negative = spend
    source          VARCHAR(50) NOT NULL,   -- lesson_complete, flashcard_review, streak_bonus, pronunciation, writing, achievement
    source_id       UUID,                   -- reference to the source entity
    description     VARCHAR(200),
    balance_after   INT NOT NULL,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_xp_user ON xp_transactions(user_id);
CREATE INDEX idx_xp_user_date ON xp_transactions(user_id, created_at);
```

### 30. achievements
```sql
CREATE TABLE achievements (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    code            VARCHAR(50) NOT NULL UNIQUE,    -- 'first_lesson', 'streak_7', 'pronunciation_90'
    title           VARCHAR(200) NOT NULL,
    description     TEXT NOT NULL,
    icon_url        VARCHAR(500),
    xp_reward       INT NOT NULL DEFAULT 0,
    category        VARCHAR(30) NOT NULL,           -- streak, lesson, pronunciation, vocabulary, social
    condition_json  JSONB NOT NULL,                 -- {"type":"streak","value":7}
    sort_order      INT NOT NULL,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 31. user_achievements
```sql
CREATE TABLE user_achievements (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    achievement_id  UUID NOT NULL REFERENCES achievements(id),
    unlocked_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(user_id, achievement_id)
);

CREATE INDEX idx_user_achievements ON user_achievements(user_id);
```

### 32. leaderboard_entries
Snapshot-based, refreshed periodically.
```sql
CREATE TABLE leaderboard_entries (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    period_type     VARCHAR(10) NOT NULL,   -- weekly, monthly, all_time
    period_key      VARCHAR(20) NOT NULL,   -- '2024-W48', '2024-12', 'all'
    total_xp        INT NOT NULL DEFAULT 0,
    rank            INT,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(user_id, period_type, period_key)
);

CREATE INDEX idx_leaderboard_period ON leaderboard_entries(period_type, period_key, total_xp DESC);
```

### 33. vocabulary_bank
User's personal vocabulary collection.
```sql
CREATE TABLE vocabulary_bank (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    word            VARCHAR(200) NOT NULL,
    definition      TEXT NOT NULL,
    phonetic        VARCHAR(200),
    part_of_speech  VARCHAR(20),
    example_sentence TEXT,
    audio_url       VARCHAR(500),
    source          VARCHAR(50),            -- reading, conversation, manual, pronunciation
    source_id       UUID,
    mastery_level   INT NOT NULL DEFAULT 0, -- 0-5
    is_favorite     BOOLEAN NOT NULL DEFAULT FALSE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(user_id, word)
);

CREATE INDEX idx_vocab_user ON vocabulary_bank(user_id);
```

### 34. daily_challenges
```sql
CREATE TABLE daily_challenges (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    challenge_date  DATE NOT NULL,
    title           VARCHAR(200) NOT NULL,
    description     TEXT NOT NULL,
    challenge_type  VARCHAR(30) NOT NULL,   -- pronunciation, grammar, vocabulary, writing
    content_json    JSONB NOT NULL,
    xp_reward       INT NOT NULL DEFAULT 20,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(challenge_date, challenge_type)
);
```

### 35. daily_challenge_completions
```sql
CREATE TABLE daily_challenge_completions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    challenge_id    UUID NOT NULL REFERENCES daily_challenges(id),
    score           DECIMAL(5,2),
    completed_at    TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE(user_id, challenge_id)
);
```

---

## EF Core Configuration Example

```csharp
// Infrastructure/Persistence/Configurations/FlashcardConfiguration.cs
public class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.ToTable("flashcards");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Front).HasMaxLength(500).IsRequired();
        builder.Property(f => f.Back).HasMaxLength(500).IsRequired();
        builder.Property(f => f.EaseFactor).HasPrecision(4, 2).HasDefaultValue(2.50m);

        builder.HasOne(f => f.Deck)
            .WithMany(d => d.Cards)
            .HasForeignKey(f => f.DeckId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(f => !f.IsDeleted);

        builder.HasIndex(f => new { f.DeckId, f.NextReviewAt })
            .HasFilter("NOT is_deleted");
    }
}
```

## Seed Data Strategy
1. **Achievements**: Seeded via migration (30+ achievements)
2. **Grammar Topics**: Seeded from JSON files (50+ topics across A1-C2)
3. **Roleplay Scenarios**: Seeded from JSON (20+ scenarios)
4. **Reading Articles**: Seeded from JSON (sample articles per level)
5. **Listening Exercises**: Seeded with audio URLs
6. **Daily Challenges**: Generated daily via background job

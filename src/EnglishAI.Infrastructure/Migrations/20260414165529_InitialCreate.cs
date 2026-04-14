using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    xp_reward = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    condition_json = table.Column<string>(type: "jsonb", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_achievements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "daily_challenges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    challenge_date = table.Column<DateOnly>(type: "date", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    challenge_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    content_json = table.Column<string>(type: "jsonb", nullable: false),
                    xp_reward = table.Column<int>(type: "integer", nullable: false, defaultValue: 20),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_challenges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grammar_topics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false),
                    examples = table.Column<string>(type: "jsonb", nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    lesson_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    content_json = table.Column<string>(type: "jsonb", nullable: false),
                    duration_min = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    xp_reward = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    difficulty = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    tags = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_ai_generated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lessons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listening_exercises",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    audio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    transcript = table.Column<string>(type: "text", nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    exercise_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    duration_sec = table.Column<int>(type: "integer", nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    questions = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listening_exercises", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading_articles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    word_count = table.Column<int>(type: "integer", nullable: false),
                    estimated_min = table.Column<int>(type: "integer", nullable: false),
                    vocabulary_list = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    comprehension_questions = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    source_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_ai_generated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_articles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roleplay_scenarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    system_prompt = table.Column<string>(type: "text", nullable: false),
                    starter_message = table.Column<string>(type: "text", nullable: false),
                    objectives = table.Column<string>(type: "jsonb", nullable: false),
                    vocabulary_hints = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roleplay_scenarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    normalized_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Learner"),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grammar_questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    options = table.Column<string>(type: "jsonb", nullable: true),
                    correct_answer = table.Column<string>(type: "text", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false),
                    difficulty = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_grammar_questions_grammar_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "grammar_topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversation_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    scenario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ai_model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "gpt-4o"),
                    system_prompt = table.Column<string>(type: "text", nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    message_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    duration_sec = table.Column<int>(type: "integer", nullable: true),
                    grammar_errors = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    vocabulary_used = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    session_feedback = table.Column<string>(type: "jsonb", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversation_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_sessions_roleplay_scenarios_scenario_id",
                        column: x => x.scenario_id,
                        principalTable: "roleplay_scenarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_conversation_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_challenge_completions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    challenge_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_challenge_completions", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_challenge_completions_daily_challenges_challenge_id",
                        column: x => x.challenge_id,
                        principalTable: "daily_challenges",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_daily_challenge_completions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dictation_attempts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_text = table.Column<string>(type: "text", nullable: false),
                    accuracy_pct = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    errors = table.Column<string>(type: "jsonb", nullable: false),
                    time_spent_sec = table.Column<int>(type: "integer", nullable: false),
                    attempted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dictation_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_dictation_attempts_listening_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "listening_exercises",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_dictation_attempts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flashcard_decks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    card_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    tags = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flashcard_decks", x => x.id);
                    table.ForeignKey(
                        name: "fk_flashcard_decks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_quiz_attempts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    total_questions = table.Column<int>(type: "integer", nullable: false),
                    correct_count = table.Column<int>(type: "integer", nullable: false),
                    time_spent_sec = table.Column<int>(type: "integer", nullable: false),
                    answers = table.Column<string>(type: "jsonb", nullable: false),
                    attempted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_quiz_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_grammar_quiz_attempts_grammar_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "grammar_topics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_grammar_quiz_attempts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leaderboard_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    period_key = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    total_xp = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    rank = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_leaderboard_entries", x => x.id);
                    table.ForeignKey(
                        name: "fk_leaderboard_entries_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learning_paths",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    progress_pct = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_learning_paths", x => x.id);
                    table.ForeignKey(
                        name: "fk_learning_paths_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lesson_completions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    time_spent_sec = table.Column<int>(type: "integer", nullable: false),
                    xp_earned = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    attempt_number = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lesson_completions", x => x.id);
                    table.ForeignKey(
                        name: "fk_lesson_completions_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lesson_completions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pronunciation_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    target_text = table.Column<string>(type: "text", nullable: false),
                    target_phonemes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    cefr_level = table.Column<int>(type: "integer", nullable: true),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pronunciation_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_pronunciation_sessions_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pronunciation_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reading_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_spent_sec = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    progress_pct = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    quiz_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    words_looked_up = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_reading_sessions_reading_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "reading_articles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_reading_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    achievement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unlocked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_achievements", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_achievements_achievements_achievement_id",
                        column: x => x.achievement_id,
                        principalTable: "achievements",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_achievements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_external_logins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    provider_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_external_logins", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_external_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    native_language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "vi"),
                    current_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false, defaultValue: "A1"),
                    target_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false, defaultValue: "B2"),
                    daily_goal_minutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 15),
                    learning_purpose = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    weak_skills = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    interests = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    onboarding_completed = table.Column<bool>(type: "boolean", nullable: false),
                    timezone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "Asia/Ho_Chi_Minh"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    daily_reminder_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    sound_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dark_mode = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    language_ui = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "vi"),
                    auto_play_audio = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    show_phonetic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_settings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_streaks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_streak = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    longest_streak = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    last_activity_date = table.Column<DateOnly>(type: "date", nullable: true),
                    freeze_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_days_learned = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_streaks", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_streaks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_bank",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    definition = table.Column<string>(type: "text", nullable: false),
                    phonetic = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    part_of_speech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    example_sentence = table.Column<string>(type: "text", nullable: true),
                    audio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    source_id = table.Column<Guid>(type: "uuid", nullable: true),
                    mastery_level = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vocabulary_bank", x => x.id);
                    table.ForeignKey(
                        name: "fk_vocabulary_bank_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "writing_submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prompt_text = table.Column<string>(type: "text", nullable: false),
                    prompt_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    cefr_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    user_text = table.Column<string>(type: "text", nullable: false),
                    word_count = table.Column<int>(type: "integer", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_writing_submissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_writing_submissions_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_writing_submissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "xp_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    source_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    balance_after = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_xp_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_xp_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversation_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    grammar_issues = table.Column<string>(type: "jsonb", nullable: true),
                    vocab_level = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    audio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    token_count = table.Column<int>(type: "integer", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversation_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_messages_conversation_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "conversation_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flashcards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    deck_id = table.Column<Guid>(type: "uuid", nullable: false),
                    front = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    back = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    phonetic = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    example_sentence = table.Column<string>(type: "text", nullable: true),
                    audio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    part_of_speech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ease_factor = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false, defaultValue: 2.50m),
                    interval_days = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    repetitions = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    next_review_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flashcards", x => x.id);
                    table.ForeignKey(
                        name: "fk_flashcards_flashcard_decks_deck_id",
                        column: x => x.deck_id,
                        principalTable: "flashcard_decks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learning_path_units",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    learning_path_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    unit_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_learning_path_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_learning_path_units_learning_paths_learning_path_id",
                        column: x => x.learning_path_id,
                        principalTable: "learning_paths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pronunciation_attempts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    audio_duration_sec = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    recognized_text = table.Column<string>(type: "text", nullable: true),
                    recognized_phonemes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    accuracy_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    fluency_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    completeness_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    pronunciation_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    phoneme_scores = table.Column<string>(type: "jsonb", nullable: true),
                    word_scores = table.Column<string>(type: "jsonb", nullable: true),
                    feedback_text = table.Column<string>(type: "text", nullable: true),
                    waveform_data = table.Column<string>(type: "jsonb", nullable: true),
                    attempted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pronunciation_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_pronunciation_attempts_pronunciation_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "pronunciation_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pronunciation_attempts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "writing_feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    overall_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    grammar_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    vocabulary_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    coherence_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    task_achievement = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    corrected_text = table.Column<string>(type: "text", nullable: false),
                    inline_corrections = table.Column<string>(type: "jsonb", nullable: false),
                    general_feedback = table.Column<string>(type: "text", nullable: false),
                    improvement_tips = table.Column<string>(type: "jsonb", nullable: false),
                    vocabulary_suggestions = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "[]"),
                    ai_model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_writing_feedback", x => x.id);
                    table.ForeignKey(
                        name: "fk_writing_feedback_writing_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "writing_submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flashcard_reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    flashcard_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quality = table.Column<int>(type: "integer", nullable: false),
                    time_spent_ms = table.Column<int>(type: "integer", nullable: false),
                    was_correct = table.Column<bool>(type: "boolean", nullable: false),
                    previous_interval = table.Column<int>(type: "integer", nullable: false),
                    new_interval = table.Column<int>(type: "integer", nullable: false),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flashcard_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_flashcard_reviews_flashcards_flashcard_id",
                        column: x => x.flashcard_id,
                        principalTable: "flashcards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_flashcard_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "unit_lessons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_unit_lessons_learning_path_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "learning_path_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_unit_lessons_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_achievements_code",
                table: "achievements",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_conv_messages_session",
                table: "conversation_messages",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "idx_conv_sessions_user",
                table: "conversation_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversation_sessions_scenario_id",
                table: "conversation_sessions",
                column: "scenario_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_challenge_completions_challenge_id",
                table: "daily_challenge_completions",
                column: "challenge_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_challenge_completions_user_id_challenge_id",
                table: "daily_challenge_completions",
                columns: new[] { "user_id", "challenge_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_daily_challenges_challenge_date_challenge_type",
                table: "daily_challenges",
                columns: new[] { "challenge_date", "challenge_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dictation_attempts_exercise_id",
                table: "dictation_attempts",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "ix_dictation_attempts_user_id",
                table: "dictation_attempts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_decks_user",
                table: "flashcard_decks",
                column: "user_id",
                filter: "NOT is_deleted");

            migrationBuilder.CreateIndex(
                name: "idx_reviews_card",
                table: "flashcard_reviews",
                column: "flashcard_id");

            migrationBuilder.CreateIndex(
                name: "idx_reviews_user",
                table: "flashcard_reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_flashcards_deck",
                table: "flashcards",
                column: "deck_id",
                filter: "NOT is_deleted");

            migrationBuilder.CreateIndex(
                name: "idx_flashcards_due",
                table: "flashcards",
                columns: new[] { "deck_id", "next_review_at" },
                filter: "NOT is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_questions_topic_id",
                table: "grammar_questions",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "idx_grammar_attempts_user",
                table: "grammar_quiz_attempts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_quiz_attempts_topic_id",
                table: "grammar_quiz_attempts",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_topics_slug",
                table: "grammar_topics",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_leaderboard_period",
                table: "leaderboard_entries",
                columns: new[] { "period_type", "period_key", "total_xp" });

            migrationBuilder.CreateIndex(
                name: "ix_leaderboard_entries_user_id_period_type_period_key",
                table: "leaderboard_entries",
                columns: new[] { "user_id", "period_type", "period_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_units_path",
                table: "learning_path_units",
                column: "learning_path_id");

            migrationBuilder.CreateIndex(
                name: "idx_learning_paths_user",
                table: "learning_paths",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_completions_lesson",
                table: "lesson_completions",
                columns: new[] { "user_id", "lesson_id" });

            migrationBuilder.CreateIndex(
                name: "idx_completions_user",
                table: "lesson_completions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_lesson_completions_lesson_id",
                table: "lesson_completions",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "idx_lessons_type_level",
                table: "lessons",
                columns: new[] { "lesson_type", "cefr_level" });

            migrationBuilder.CreateIndex(
                name: "idx_pron_attempts_session",
                table: "pronunciation_attempts",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "idx_pron_attempts_user",
                table: "pronunciation_attempts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_pron_sessions_user",
                table: "pronunciation_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_pronunciation_sessions_lesson_id",
                table: "pronunciation_sessions",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "idx_articles_level",
                table: "reading_articles",
                column: "cefr_level");

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_article_id",
                table: "reading_sessions",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_user_id",
                table: "reading_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token");

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_user",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_scenarios_level",
                table: "roleplay_scenarios",
                column: "cefr_level");

            migrationBuilder.CreateIndex(
                name: "ix_unit_lessons_lesson_id",
                table: "unit_lessons",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "ix_unit_lessons_unit_id_lesson_id",
                table: "unit_lessons",
                columns: new[] { "unit_id", "lesson_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_achievements",
                table: "user_achievements",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_user_id_achievement_id",
                table: "user_achievements",
                columns: new[] { "user_id", "achievement_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_external_logins_provider_provider_key",
                table: "user_external_logins",
                columns: new[] { "provider", "provider_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_external_logins_user_id",
                table: "user_external_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_streaks_user_id",
                table: "user_streaks",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_users_email",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "idx_vocab_user",
                table: "vocabulary_bank",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_vocabulary_bank_user_id_word",
                table: "vocabulary_bank",
                columns: new[] { "user_id", "word" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_writing_feedback_submission_id",
                table: "writing_feedback",
                column: "submission_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_writing_user",
                table: "writing_submissions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_writing_submissions_lesson_id",
                table: "writing_submissions",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "idx_xp_user",
                table: "xp_transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_xp_user_date",
                table: "xp_transactions",
                columns: new[] { "user_id", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conversation_messages");

            migrationBuilder.DropTable(
                name: "daily_challenge_completions");

            migrationBuilder.DropTable(
                name: "dictation_attempts");

            migrationBuilder.DropTable(
                name: "flashcard_reviews");

            migrationBuilder.DropTable(
                name: "grammar_questions");

            migrationBuilder.DropTable(
                name: "grammar_quiz_attempts");

            migrationBuilder.DropTable(
                name: "leaderboard_entries");

            migrationBuilder.DropTable(
                name: "lesson_completions");

            migrationBuilder.DropTable(
                name: "pronunciation_attempts");

            migrationBuilder.DropTable(
                name: "reading_sessions");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "unit_lessons");

            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "user_external_logins");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "user_streaks");

            migrationBuilder.DropTable(
                name: "vocabulary_bank");

            migrationBuilder.DropTable(
                name: "writing_feedback");

            migrationBuilder.DropTable(
                name: "xp_transactions");

            migrationBuilder.DropTable(
                name: "conversation_sessions");

            migrationBuilder.DropTable(
                name: "daily_challenges");

            migrationBuilder.DropTable(
                name: "listening_exercises");

            migrationBuilder.DropTable(
                name: "flashcards");

            migrationBuilder.DropTable(
                name: "grammar_topics");

            migrationBuilder.DropTable(
                name: "pronunciation_sessions");

            migrationBuilder.DropTable(
                name: "reading_articles");

            migrationBuilder.DropTable(
                name: "learning_path_units");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "writing_submissions");

            migrationBuilder.DropTable(
                name: "roleplay_scenarios");

            migrationBuilder.DropTable(
                name: "flashcard_decks");

            migrationBuilder.DropTable(
                name: "learning_paths");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

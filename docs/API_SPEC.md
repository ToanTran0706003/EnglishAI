# API Specification — EnglishAI

## Base URL
```
Production:  https://api.englishai.dev/api/v1
Development: https://localhost:5001/api/v1
```

## Authentication
All protected endpoints require: `Authorization: Bearer <jwt_token>`

## Standard Response Format
```json
// Success
{
  "success": true,
  "data": { ... },
  "meta": { "page": 1, "pageSize": 20, "totalCount": 150 }
}

// Error
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "One or more validation errors occurred.",
    "details": [
      { "field": "email", "message": "Email is required." }
    ]
  }
}
```

---

## 1. Auth Endpoints

### POST /auth/register
```
Body: { email, password, displayName }
Response 201: { user, accessToken, refreshToken (cookie) }
Validation: email format, password min 8 chars + 1 upper + 1 number
```

### POST /auth/login
```
Body: { email, password }
Response 200: { user, accessToken }
+ Set-Cookie: refreshToken (httpOnly, secure, sameSite=strict)
```

### POST /auth/login/google
```
Body: { idToken }
Response 200: { user, accessToken, isNewUser }
```

### POST /auth/refresh
```
Cookie: refreshToken
Response 200: { accessToken }
+ Set-Cookie: new refreshToken (rotation)
```

### POST /auth/logout
```
Response 204
Revokes refresh token, clears cookie
```

### POST /auth/forgot-password
```
Body: { email }
Response 200: { message }
Sends reset email
```

### POST /auth/reset-password
```
Body: { token, newPassword }
Response 200: { message }
```

### GET /auth/me
```
[Authorize]
Response 200: { user + profile + settings }
```

---

## 2. Onboarding Endpoints

### PUT /onboarding/profile
```
[Authorize]
Body: {
  nativeLanguage: "vi",
  currentLevel: "A2",
  targetLevel: "B2",
  dailyGoalMinutes: 15,
  learningPurpose: "work",
  interests: ["technology", "travel"]
}
Response 200: { profile }
```

### POST /onboarding/placement-test
```
[Authorize]
Body: { answers: [{ questionId, answer }] }
Response 200: { recommendedLevel: "B1", scores: { grammar: 72, vocabulary: 65, reading: 80 } }
```

### POST /onboarding/complete
```
[Authorize]
Generates learning path based on profile
Response 200: { learningPath }
```

---

## 3. Dashboard Endpoints

### GET /dashboard
```
[Authorize]
Response 200: {
  streak: { current: 7, longest: 14 },
  todayProgress: { minutesLearned: 12, goalMinutes: 15, lessonsCompleted: 2 },
  xp: { total: 2450, todayEarned: 80, level: 12 },
  dueFlashcards: 15,
  currentLesson: { id, title, type, progress },
  weeklyActivity: [{ date, minutes, xp }],
  achievements: { recent: [...], total: 8 }
}
```

### GET /dashboard/activity?days=30
```
[Authorize]
Response 200: { activities: [{ date, type, duration, xp, details }] }
```

---

## 4. Learning Path Endpoints

### GET /learning-paths
```
[Authorize]
Response 200: { paths: [{ id, title, level, progress, units }] }
```

### GET /learning-paths/{id}
```
[Authorize]
Response 200: { path with units and lessons, completion status }
```

### GET /learning-paths/{pathId}/units/{unitId}/lessons
```
[Authorize]
Response 200: { lessons: [{ id, title, type, duration, isCompleted, score }] }
```

### POST /learning-paths/{pathId}/regenerate
```
[Authorize]
AI regenerates path based on current progress
Response 200: { newPath }
```

---

## 5. Lesson Endpoints

### GET /lessons/{id}
```
[Authorize]
Response 200: { lesson with full content_json }
```

### POST /lessons/{id}/start
```
[Authorize]
Response 200: { sessionId, content }
```

### POST /lessons/{id}/complete
```
[Authorize]
Body: { score, timeSpentSec, answers? }
Response 200: { xpEarned, newAchievements, nextLesson }
```

---

## 6. Flashcard Endpoints

### GET /flashcard-decks
```
[Authorize]
Query: ?page=1&pageSize=20
Response 200: { decks: [{ id, title, cardCount, dueCount, lastStudied }] }
```

### POST /flashcard-decks
```
[Authorize]
Body: { title, description, cefrLevel }
Response 201: { deck }
```

### GET /flashcard-decks/{id}/cards
```
[Authorize]
Query: ?filter=due|all|new
Response 200: { cards: [...] }
```

### POST /flashcard-decks/{id}/cards
```
[Authorize]
Body: { front, back, phonetic?, exampleSentence?, audioUrl? }
Response 201: { card }
```

### POST /flashcard-decks/{id}/cards/bulk
```
[Authorize]
Body: { cards: [{ front, back, ... }] }
Response 201: { cards, count }
```

### POST /flashcards/{id}/review
```
[Authorize]
Body: { quality: 4, timeSpentMs: 3500 }  // quality 0-5 (SM-2)
Response 200: { nextReviewAt, intervalDays, easeFactor }
```

### GET /flashcards/due
```
[Authorize]
Query: ?deckId=xxx&limit=20
Response 200: { cards: [...], totalDue: 45 }
```

### POST /flashcards/generate
```
[Authorize]
Body: { topic: "business meeting", count: 10, cefrLevel: "B2" }
AI generates flashcards
Response 200: { cards: [...] }
```

---

## 7. Pronunciation Endpoints ⭐ (Priority Feature)

### POST /pronunciation/sessions
```
[Authorize]
Body: {
  sessionType: "sentence",     // word, sentence, minimal_pair, free_speech
  targetText: "The weather is beautiful today",
  cefrLevel: "B1"
}
Response 201: {
  sessionId: "uuid",
  targetText: "The weather is beautiful today",
  targetPhonemes: "ðə ˈwɛðər ɪz ˈbjuːtɪfəl təˈdeɪ",
  referenceAudioUrl: "/audio/ref/xxx.mp3"   // TTS reference
}
```

### POST /pronunciation/sessions/{sessionId}/attempts
```
[Authorize]
Content-Type: multipart/form-data
Body: { audio: <wav/webm file, max 30s> }
Response 200: {
  attemptId: "uuid",
  recognizedText: "The weather is beautiful today",
  scores: {
    accuracy: 78.5,
    fluency: 82.0,
    completeness: 100.0,
    pronunciation: 80.2
  },
  phonemeScores: [
    { phoneme: "ð", score: 65, offset: 0.1, duration: 0.08 },
    { phoneme: "ə", score: 90, offset: 0.18, duration: 0.05 },
    ...
  ],
  wordScores: [
    { word: "The", score: 72, errorType: null },
    { word: "weather", score: 68, errorType: "mispronunciation" },
    ...
  ],
  feedback: "Focus on the 'th' sound /ð/. Place your tongue between your teeth.",
  waveformData: {
    reference: [0.1, 0.3, 0.5, ...],
    user: [0.08, 0.25, 0.6, ...]
  }
}
```

### GET /pronunciation/sessions/{sessionId}/attempts
```
[Authorize]
Response 200: { attempts: [...] }  // history for this session
```

### GET /pronunciation/history
```
[Authorize]
Query: ?days=30&type=sentence
Response 200: {
  sessions: [...],
  stats: {
    avgScore: 75.3,
    totalSessions: 42,
    improvementPct: 12.5,
    weakPhonemes: ["ð", "θ", "æ"]
  }
}
```

### GET /pronunciation/minimal-pairs
```
[Authorize]
Query: ?cefrLevel=B1&targetPhonemes=ð,θ
Response 200: {
  pairs: [
    { word1: "then", phoneme1: "ðɛn", word2: "thin", phoneme2: "θɪn", audioUrl1: "...", audioUrl2: "..." },
    ...
  ]
}
```

---

## 8. Conversation AI Endpoints

### POST /conversations/sessions
```
[Authorize]
Body: {
  sessionType: "free_chat",    // free_chat, roleplay
  scenarioId?: "uuid",         // for roleplay
  cefrLevel: "B1"
}
Response 201: {
  sessionId: "uuid",
  starterMessage: "Hi! What would you like to talk about today?",
  systemPrompt: "..."  // (only for debug)
}
```

### POST /conversations/sessions/{id}/messages
```
[Authorize]
Body: { content: "I want to discuss my weekend plans" }
Response 200: {
  userMessage: { id, content, grammarIssues: [...] },
  assistantMessage: { id, content },
  corrections: [
    { original: "I go to beach", corrected: "I went to the beach", type: "tense" }
  ]
}
```

### POST /conversations/sessions/{id}/messages/voice
```
[Authorize]
Content-Type: multipart/form-data
Body: { audio: <file> }
Response 200: { transcription, ...same as above }
```

### POST /conversations/sessions/{id}/end
```
[Authorize]
Response 200: {
  feedback: {
    grammarScore: 72,
    vocabularyScore: 68,
    fluencyScore: 80,
    overallScore: 73,
    tips: ["Try using more varied tenses", "Good use of connectors"],
    newVocabSuggestions: ["moreover", "nevertheless"],
    grammarErrorSummary: { tense: 3, article: 2, preposition: 1 }
  }
}
```

### GET /conversations/scenarios
```
Query: ?cefrLevel=B1&category=travel
Response 200: { scenarios: [...] }
```

---

## 9. Writing Endpoints

### GET /writing/prompts
```
[Authorize]
Query: ?type=email&cefrLevel=B1
Response 200: { prompts: [{ id, text, type, cefrLevel, wordLimit }] }
```

### POST /writing/submit
```
[Authorize]
Body: {
  promptText: "Write an email to your boss requesting a day off",
  promptType: "email",
  userText: "Dear Mr. Smith, I writing to request..."
}
Response 200: {
  submissionId: "uuid",
  feedback: {
    overallScore: 68,
    grammarScore: 55,
    vocabularyScore: 72,
    coherenceScore: 75,
    taskAchievement: 70,
    correctedText: "Dear Mr. Smith, I am writing to request...",
    inlineCorrections: [
      { offset: 25, length: 7, type: "grammar", original: "writing", correction: "am writing", explanation: "Use present continuous 'am writing' for formal letters" }
    ],
    generalFeedback: "Good structure, but watch your tense consistency.",
    improvementTips: ["Use more formal connectors", "Add a closing line"]
  }
}
```

---

## 10. Reading Endpoints

### GET /reading/articles
```
Query: ?cefrLevel=B1&category=technology&page=1
Response 200: { articles: [{ id, title, summary, level, category, wordCount, estimatedMin }] }
```

### GET /reading/articles/{id}
```
[Authorize]
Response 200: { article with full content, vocabulary list, questions }
```

### POST /reading/articles/{id}/sessions
```
[Authorize]
Body: { timeSpentSec, progressPct, quizAnswers?, wordsLookedUp? }
Response 200: { quizScore, xpEarned }
```

### POST /reading/articles/{id}/ask-ai
```
[Authorize]
Body: { question: "What does 'ubiquitous' mean in this context?" }
AI reading assistant
Response 200: { answer, relatedVocabulary }
```

---

## 11. Listening Endpoints

### GET /listening/exercises
```
Query: ?cefrLevel=B1&type=dictation
Response 200: { exercises: [...] }
```

### POST /listening/exercises/{id}/attempt
```
[Authorize]
Body: { userText: "..." }
Response 200: { accuracyPct, errors: [...], xpEarned }
```

---

## 12. Grammar Endpoints

### GET /grammar/topics
```
Query: ?cefrLevel=B1&category=tenses
Response 200: { topics: [{ id, title, slug, level, category }] }
```

### GET /grammar/topics/{slug}
```
Response 200: { topic with explanation, examples }
```

### GET /grammar/topics/{id}/quiz
```
[Authorize]
Query: ?count=10
Response 200: { questions: [...] }
```

### POST /grammar/topics/{id}/quiz/submit
```
[Authorize]
Body: { answers: [{ questionId, answer }] }
Response 200: { score, correctCount, totalQuestions, xpEarned, detailedResults }
```

### POST /grammar/check
```
[Authorize]
Body: { text: "I go to school yesterday" }
AI grammar checker
Response 200: {
  corrections: [
    { original: "go", corrected: "went", type: "tense", explanation: "Use past simple for completed actions" }
  ],
  correctedText: "I went to school yesterday"
}
```

---

## 13. Progress & Stats Endpoints

### GET /progress/overview
```
[Authorize]
Response 200: {
  totalXp: 2450,
  currentLevel: 12,
  xpToNextLevel: 150,
  streak: { current: 7, longest: 14, freezesLeft: 1 },
  skillBreakdown: {
    grammar: { score: 72, trend: "up" },
    vocabulary: { score: 68, trend: "stable" },
    pronunciation: { score: 65, trend: "up" },
    reading: { score: 80, trend: "up" },
    listening: { score: 60, trend: "down" },
    writing: { score: 70, trend: "stable" }
  },
  weeklyGoal: { target: 105, completed: 78, unit: "minutes" },
  recentActivity: [...]
}
```

### GET /progress/history
```
[Authorize]
Query: ?period=30d|90d|1y
Response 200: { dataPoints: [{ date, xp, minutes, lessonsCompleted }] }
```

---

## 14. Gamification Endpoints

### GET /achievements
```
[Authorize]
Response 200: { unlocked: [...], locked: [...], totalUnlocked: 8, totalAvailable: 30 }
```

### GET /leaderboard
```
Query: ?period=weekly|monthly|all_time&page=1
Response 200: {
  entries: [{ rank, userId, displayName, avatarUrl, xp }],
  currentUser: { rank: 42, xp: 2450 }
}
```

### GET /xp/transactions
```
[Authorize]
Query: ?page=1&source=lesson_complete
Response 200: { transactions: [...] }
```

---

## 15. Vocabulary Bank Endpoints

### GET /vocabulary
```
[Authorize]
Query: ?search=&masteryLevel=0-5&sort=recent|alphabetical
Response 200: { words: [...], totalCount }
```

### POST /vocabulary
```
[Authorize]
Body: { word, definition, exampleSentence?, source? }
Response 201: { vocabularyEntry }
```

### PATCH /vocabulary/{id}/mastery
```
[Authorize]
Body: { masteryLevel: 3 }
Response 200: { updated }
```

---

## 16. Profile & Settings Endpoints

### PUT /users/profile
```
[Authorize]
Body: { displayName, avatarUrl, currentLevel, targetLevel, dailyGoalMinutes }
Response 200: { profile }
```

### PUT /users/settings
```
[Authorize]
Body: { notificationEnabled, darkMode, soundEnabled, ... }
Response 200: { settings }
```

### PUT /users/avatar
```
[Authorize]
Content-Type: multipart/form-data
Body: { avatar: <image file> }
Response 200: { avatarUrl }
```

### DELETE /users/account
```
[Authorize]
Body: { password }
Response 204
Soft delete + schedule data cleanup
```

---

## 17. Admin Endpoints (Role: Admin)

### GET /admin/users
### GET /admin/lessons
### POST /admin/lessons
### PUT /admin/lessons/{id}
### POST /admin/articles
### POST /admin/listening-exercises
### GET /admin/stats
```
Dashboard metrics: DAU, MAU, retention, popular lessons, etc.
```

---

## SignalR Hub Endpoints

### /hubs/pronunciation
```
Client → Server:
  SendAudioChunk(byte[] audioData)
  StartSession(Guid sessionId)
  EndSession(Guid sessionId)

Server → Client:
  ReceivePartialResult(PartialPronunciationResult)
  ReceiveFinalResult(PronunciationResult)
  ReceiveError(string error)
```

### /hubs/conversation
```
Client → Server:
  SendMessage(string message, Guid sessionId)
  SendVoiceMessage(byte[] audio, Guid sessionId)

Server → Client:
  ReceiveChunk(string textChunk)
  ReceiveComplete(ConversationResponse)
  ReceiveGrammarHint(GrammarCorrection)
```

### /hubs/notifications
```
Server → Client:
  ReceiveAchievement(AchievementDto)
  ReceiveStreakReminder()
  ReceiveXpUpdate(int newTotal)
  ReceiveDailyChallenge(DailyChallengeDto)
```

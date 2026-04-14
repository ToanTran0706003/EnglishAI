# Feature Specifications — EnglishAI

## Feature Priority Matrix

| Priority | Feature | Complexity | Sprint |
|----------|---------|-----------|--------|
| P0 | Auth (JWT + Google OAuth) | Medium | 1 |
| P0 | User Profile + Onboarding | Medium | 1 |
| P0 | Dashboard | Medium | 2 |
| P0 | Learning Path | High | 2-3 |
| P0 | Flashcard + SRS | High | 3 |
| P0 | Grammar Lesson + Quiz | Medium | 4 |
| P0 | Pronunciation AI ⭐ | Very High | 4-5 |
| P0 | XP + Achievements | Medium | 5 |
| P1 | Reading + AI Assistant | High | 6 |
| P1 | Listening + Dictation | Medium | 6 |
| P1 | Writing + AI Feedback | High | 7 |
| P1 | AI Conversation | Very High | 7-8 |
| P1 | Roleplay Scenarios | High | 8 |
| P1 | Minimal Pairs Drill | Medium | 8 |
| P2 | Vocabulary Bank | Low | 9 |
| P2 | Leaderboard | Low | 9 |
| P2 | Daily Challenges | Medium | 9 |
| P2 | Admin CMS | High | 10 |
| P2 | Session Feedback Reports | Medium | 10 |

---

## Feature Specs

### F01: Authentication System

**User Stories:**
- As a user, I can register with email/password
- As a user, I can login with Google
- As a user, I can reset my password
- As a user, I stay logged in via refresh tokens

**Acceptance Criteria:**
- [x] Email validation + password strength (min 8, upper+number)
- [x] JWT 15min expiry + refresh token 7 days in httpOnly cookie
- [x] Refresh token rotation (old token revoked on use)
- [x] Google OAuth 2.0 integration
- [x] Email verification flow
- [x] Rate limiting: 5 login attempts per 15min per IP
- [x] Account lockout after 10 failed attempts

**Technical Notes:**
- Use ASP.NET Identity for user management
- BCrypt for password hashing
- Google Authentication via `Microsoft.AspNetCore.Authentication.Google`
- Store refresh tokens in DB with IP tracking

---

### F02: Onboarding Wizard

**User Stories:**
- As a new user, I complete a wizard to personalize my learning
- As a user, I can take a placement test to determine my level

**Acceptance Criteria:**
- [ ] Step 1: Native language selection
- [ ] Step 2: Current CEFR level (self-assessment or test)
- [ ] Step 3: Target level
- [ ] Step 4: Daily goal (5/10/15/30/60 min)
- [ ] Step 5: Learning purpose (exam/travel/work/hobby)
- [ ] Step 6: Interests for content personalization
- [ ] Optional placement test (20 questions, grammar + vocabulary)
- [ ] Auto-generate initial learning path on completion

---

### F03: Dashboard

**Acceptance Criteria:**
- [ ] Current streak display (with freeze indicator)
- [ ] Today's progress bar (minutes toward daily goal)
- [ ] XP earned today + total level
- [ ] Continue learning CTA (current lesson)
- [ ] Due flashcard count
- [ ] 7-day activity heatmap
- [ ] Recent achievements
- [ ] Quick access to all skill areas

**API:** Single `GET /dashboard` call, cached 5min in Redis.

---

### F04: Learning Path

**Acceptance Criteria:**
- [ ] Auto-generated based on user level + goals
- [ ] Tree/map visualization of units
- [ ] Units unlock sequentially
- [ ] Each unit has 3-8 lessons of mixed types
- [ ] Progress percentage per unit and overall
- [ ] "Regenerate path" button (AI-powered)
- [ ] Skill-specific paths (grammar track, pronunciation track)

**Algorithm:**
```
1. Determine user level (A1-C2)
2. Select appropriate grammar topics, vocabulary sets, pronunciation targets
3. Create units grouping related content
4. Order by prerequisite dependencies
5. Mix lesson types within each unit (grammar → vocab → practice → pronunciation)
6. Set XP requirements for unit unlock
```

---

### F05: Flashcard + Spaced Repetition (SRS)

**Acceptance Criteria:**
- [ ] Create custom decks
- [ ] Add cards manually (front/back/example/audio)
- [ ] AI card generation from topic
- [ ] SM-2 algorithm for scheduling reviews
- [ ] Review session with 4 quality buttons (Again/Hard/Good/Easy)
- [ ] Card flipping animation
- [ ] Due card count per deck
- [ ] Stats: retention rate, cards learned, review forecast
- [ ] Bulk import from CSV

**SM-2 Algorithm:** See [SRS_ALGORITHM.md](SRS_ALGORITHM.md)

---

### F06: Grammar Lessons + Quiz

**Acceptance Criteria:**
- [ ] Topic list organized by CEFR level + category
- [ ] Rich lesson content (explanation + examples + translations)
- [ ] Interactive quiz (multiple choice, fill blank, reorder, error correction)
- [ ] Immediate feedback per question with explanation
- [ ] Score + XP on completion
- [ ] Track mastery per topic
- [ ] AI grammar checker (paste any text → get corrections)

---

### F07: Pronunciation AI ⭐

**Acceptance Criteria:**
- [ ] Record audio in browser (mic permission)
- [ ] Word-level and sentence-level practice
- [ ] Real-time waveform visualization while recording
- [ ] Phoneme-level accuracy scoring (0-100)
- [ ] Fluency + completeness + overall scores
- [ ] Visual comparison: user waveform vs reference waveform
- [ ] Color-coded word highlighting (green=good, yellow=ok, red=poor)
- [ ] Phoneme-level breakdown with IPA symbols
- [ ] AI-generated pronunciation tips
- [ ] Attempt history with score trend chart
- [ ] Reference audio playback (TTS)
- [ ] Retry button

**See:** [PRONUNCIATION_AI.md](PRONUNCIATION_AI.md) for full technical spec.

---

### F08: Minimal Pairs Drill

**Acceptance Criteria:**
- [ ] Present two similar words (ship/sheep)
- [ ] Play reference audio for both
- [ ] User records one word → system detects which one
- [ ] Score based on correct phoneme production
- [ ] Focus on Vietnamese learner common confusions
- [ ] Adaptive difficulty (focus on weak phonemes)

---

### F09: AI Free Conversation

**Acceptance Criteria:**
- [ ] Start free chat with AI tutor
- [ ] AI adapts to user's CEFR level
- [ ] Real-time grammar correction hints
- [ ] Vocabulary suggestions
- [ ] Voice input option (STT → chat)
- [ ] Conversation history saved
- [ ] End-of-session feedback report
- [ ] Token tracking per user (rate limit)

---

### F10: Roleplay Scenarios

**Acceptance Criteria:**
- [ ] Scenario cards with title, description, objectives
- [ ] Categories: daily life, travel, business, interview, academic
- [ ] AI plays a character (waiter, interviewer, etc.)
- [ ] Objective checklist during conversation
- [ ] Vocabulary hints shown as needed
- [ ] Scenario completion with feedback
- [ ] New scenarios generated by AI

---

### F11: Writing Exercise + AI Feedback

**Acceptance Criteria:**
- [ ] Writing prompts by type and level
- [ ] Rich text editor
- [ ] Word count + timer
- [ ] AI feedback: overall score + 4 sub-scores
- [ ] Inline corrections with highlights
- [ ] Corrected version comparison (diff view)
- [ ] Improvement tips
- [ ] History of submissions with score trend

---

### F12: Reading + AI Assistant

**Acceptance Criteria:**
- [ ] Article list by level and category
- [ ] Reading view with vocabulary lookup (tap word → definition)
- [ ] Comprehension quiz after reading
- [ ] AI reading assistant (ask questions about the article)
- [ ] Time tracking + reading speed calculation
- [ ] Auto-add looked-up words to vocabulary bank

---

### F13: Listening + Dictation

**Acceptance Criteria:**
- [ ] Audio player with speed control (0.5x, 0.75x, 1x, 1.25x)
- [ ] Dictation mode: listen → type what you hear
- [ ] Word-by-word accuracy comparison
- [ ] Error highlighting (wrong/missing/extra words)
- [ ] Comprehension questions
- [ ] Progressive difficulty

---

### F14: XP + Achievements + Gamification

**Acceptance Criteria:**
- [ ] XP earned for every activity
- [ ] User level system (level up every 200 XP)
- [ ] 30+ achievements with categories
- [ ] Achievement popup notification (SignalR)
- [ ] Daily streak tracking
- [ ] Streak freeze (earned or purchased)
- [ ] Daily challenge
- [ ] Weekly/monthly leaderboard

**XP Table:**
| Activity | XP |
|----------|-----|
| Complete lesson | 10-20 |
| Flashcard review (10 cards) | 5 |
| Pronunciation attempt | 5 |
| Writing submission | 15 |
| Conversation session (5min+) | 10 |
| Daily challenge | 20 |
| Streak milestone (7/30/100) | 50/100/500 |
| Perfect pronunciation (95+) | 25 |

---

### F15: Profile & Settings

**Acceptance Criteria:**
- [ ] View/edit display name, avatar
- [ ] Change CEFR level, daily goal, target
- [ ] Notification preferences
- [ ] Dark mode toggle
- [ ] UI language (Vietnamese/English)
- [ ] Sound effects toggle
- [ ] Delete account
- [ ] Export my data (GDPR-lite)

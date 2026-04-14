# EnglishAI — AI-Powered English Learning Platform

## 🎯 Project Overview

**EnglishAI** is a full-stack English learning platform with deep AI integration — pronunciation scoring, AI conversation, grammar checking, and adaptive learning paths. Built as a production-grade portfolio project demonstrating advanced software engineering skills.

### Key Differentiators
- **Pronunciation AI** with phoneme-level scoring + waveform visualization
- **CQRS architecture** with MediatR for clean separation
- **Real-time features** via SignalR (live feedback, multiplayer)
- **Spaced Repetition System (SRS)** for vocabulary retention
- **Multi-tenant AI** — Azure Speech, OpenAI/Anthropic, Whisper

---

## 📁 Project Structure

```
EnglishAI/
├── src/
│   ├── EnglishAI.API/                    # ASP.NET Core 8 Web API
│   │   ├── Controllers/
│   │   ├── Hubs/                         # SignalR hubs
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   └── Program.cs
│   ├── EnglishAI.Application/            # CQRS — Commands, Queries, Handlers
│   │   ├── Common/
│   │   │   ├── Behaviors/                # Pipeline behaviors (validation, logging)
│   │   │   ├── Interfaces/
│   │   │   ├── Mappings/
│   │   │   └── Models/
│   │   ├── Features/
│   │   │   ├── Auth/
│   │   │   ├── Users/
│   │   │   ├── Lessons/
│   │   │   ├── Flashcards/
│   │   │   ├── Pronunciation/
│   │   │   ├── Conversations/
│   │   │   ├── Writing/
│   │   │   ├── Reading/
│   │   │   ├── Listening/
│   │   │   ├── Grammar/
│   │   │   ├── Progress/
│   │   │   └── Gamification/
│   │   └── DependencyInjection.cs
│   ├── EnglishAI.Domain/                 # Entities, Value Objects, Enums
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   ├── Events/
│   │   └── Common/
│   ├── EnglishAI.Infrastructure/         # EF Core, Redis, External APIs
│   │   ├── Persistence/
│   │   │   ├── Configurations/           # Entity type configurations
│   │   │   ├── Migrations/
│   │   │   ├── Interceptors/
│   │   │   └── AppDbContext.cs
│   │   ├── Services/
│   │   │   ├── AI/
│   │   │   │   ├── PronunciationService.cs
│   │   │   │   ├── ConversationService.cs
│   │   │   │   ├── GrammarCheckService.cs
│   │   │   │   └── WritingFeedbackService.cs
│   │   │   ├── Speech/
│   │   │   │   ├── AzureSpeechService.cs
│   │   │   │   └── WhisperService.cs
│   │   │   ├── Auth/
│   │   │   ├── Email/
│   │   │   └── Storage/
│   │   ├── Caching/
│   │   │   └── RedisCacheService.cs
│   │   └── DependencyInjection.cs
│   └── EnglishAI.Shared/                # DTOs, Constants, Extensions
│       ├── DTOs/
│       ├── Constants/
│       └── Extensions/
├── tests/
│   ├── EnglishAI.UnitTests/
│   ├── EnglishAI.IntegrationTests/
│   └── EnglishAI.ArchTests/
├── docker/
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
├── .github/
│   └── workflows/
│       ├── ci.yml
│       └── cd.yml
├── docs/                                 # ← YOU ARE HERE
│   ├── architecture/
│   ├── database/
│   ├── api/
│   ├── features/
│   └── devops/
├── EnglishAI.sln
└── README.md
```

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|-----------|
| **API** | ASP.NET Core 8, C# 12 |
| **Architecture** | Clean Architecture + CQRS (MediatR) |
| **ORM** | Entity Framework Core 8 |
| **Database** | PostgreSQL 16 |
| **Cache** | Redis 7 |
| **Real-time** | SignalR |
| **Auth** | JWT + Refresh Token + Google OAuth |
| **AI/Speech** | Azure Cognitive Speech, OpenAI API, Whisper |
| **Storage** | Azure Blob Storage / MinIO (local) |
| **Validation** | FluentValidation |
| **Mapping** | AutoMapper / Mapster |
| **Testing** | xUnit + FluentAssertions + Moq + Testcontainers |
| **Logging** | Serilog + Seq |
| **Docs** | Swagger / Scalar |
| **Containerization** | Docker + Docker Compose |
| **CI/CD** | GitHub Actions |
| **Deploy** | Azure App Service / Railway |

---

## 📖 Documentation Index

| Document | Description |
|----------|-------------|
| [Architecture Overview](docs/architecture/ARCHITECTURE.md) | Clean Architecture + CQRS deep dive |
| [Database Schema](docs/database/DATABASE_SCHEMA.md) | Full PostgreSQL schema with 30+ tables |
| [API Specification](docs/api/API_SPEC.md) | All endpoints, request/response models |
| [Pronunciation AI Flow](docs/features/PRONUNCIATION_AI.md) | Technical flow for pronunciation scoring |
| [Feature Specs](docs/features/FEATURES.md) | All features with acceptance criteria |
| [SRS Algorithm](docs/features/SRS_ALGORITHM.md) | Spaced repetition implementation |
| [AI Integration](docs/features/AI_INTEGRATION.md) | All AI service integrations |
| [DevOps](docs/devops/DEVOPS.md) | Docker, CI/CD, deployment |
| [TODO Plan](TODO.md) | Sprint-based development plan |

---

## 🚀 Quick Start

```bash
# Clone
git clone https://github.com/YOUR_USERNAME/EnglishAI.git
cd EnglishAI

# Start infrastructure
docker compose up -d postgres redis seq minio

# Run API
cd src/EnglishAI.API
dotnet run

# API available at https://localhost:5001
# Swagger at https://localhost:5001/swagger
# Seq logs at http://localhost:5341
```

# DevOps — EnglishAI

## Docker Compose (Development)

```yaml
# docker-compose.yml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: docker/Dockerfile
    ports:
      - "5001:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=englishai;Username=postgres;Password=0833114099
      - ConnectionStrings__Redis=redis:6379
      - AzureSpeech__SubscriptionKey=${AZURE_SPEECH_KEY}
      - AzureSpeech__Region=${AZURE_SPEECH_REGION}
      - OpenAI__ApiKey=${OPENAI_API_KEY}
      - Jwt__Secret=${JWT_SECRET}
      - Jwt__Issuer=EnglishAI
      - Jwt__Audience=EnglishAI
      - Google__ClientId=${GOOGLE_CLIENT_ID}
      - Google__ClientSecret=${GOOGLE_CLIENT_SECRET}
      - Storage__ConnectionString=http://minio:9000
      - Storage__AccessKey=minioadmin
      - Storage__SecretKey=minioadmin123
      - Seq__ServerUrl=http://seq:5341
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy
    networks:
      - englishai

  postgres:
    image: postgres:16-alpine
    ports:
      - "5432:5432"
    environment:
      POSTGRES_DB: englishai
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - englishai

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - englishai

  seq:
    image: datalust/seq:latest
    ports:
      - "5341:80"
    environment:
      ACCEPT_EULA: "Y"
    volumes:
      - seq_data:/data
    networks:
      - englishai

  minio:
    image: minio/minio:latest
    ports:
      - "9000:9000"
      - "9001:9001"
    environment:
      MINIO_ROOT_USER: minioadmin
      MINIO_ROOT_PASSWORD: minioadmin123
    volumes:
      - minio_data:/data
    command: server /data --console-address ":9001"
    networks:
      - englishai

volumes:
  postgres_data:
  redis_data:
  seq_data:
  minio_data:

networks:
  englishai:
    driver: bridge
```

## Dockerfile

```dockerfile
# docker/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY EnglishAI.sln .
COPY src/EnglishAI.API/*.csproj src/EnglishAI.API/
COPY src/EnglishAI.Application/*.csproj src/EnglishAI.Application/
COPY src/EnglishAI.Domain/*.csproj src/EnglishAI.Domain/
COPY src/EnglishAI.Infrastructure/*.csproj src/EnglishAI.Infrastructure/
COPY src/EnglishAI.Shared/*.csproj src/EnglishAI.Shared/
RUN dotnet restore

# Copy everything and build
COPY . .
RUN dotnet publish src/EnglishAI.API -c Release -o /app/publish --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "EnglishAI.API.dll"]
```

---

## GitHub Actions CI/CD

```yaml
# .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    services:
      postgres:
        image: postgres:16-alpine
        env:
          POSTGRES_DB: englishai_test
          POSTGRES_USER: postgres
          POSTGRES_PASSWORD: postgres123
        ports:
          - 5432:5432
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

      redis:
        image: redis:7-alpine
        ports:
          - 6379:6379
        options: >-
          --health-cmd "redis-cli ping"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Restore
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore --configuration Release

      - name: Unit Tests
        run: dotnet test tests/EnglishAI.UnitTests --no-build -c Release --logger "trx"

      - name: Integration Tests
        run: dotnet test tests/EnglishAI.IntegrationTests --no-build -c Release --logger "trx"
        env:
          ConnectionStrings__DefaultConnection: "Host=localhost;Database=englishai_test;Username=postgres;Password=0833114099"
          ConnectionStrings__Redis: "localhost:6379"

      - name: Architecture Tests
        run: dotnet test tests/EnglishAI.ArchTests --no-build -c Release

      - name: Publish Test Results
        uses: dorny/test-reporter@v1
        if: always()
        with:
          name: Test Results
          path: '**/*.trx'
          reporter: dotnet-trx
```

```yaml
# .github/workflows/cd.yml
name: CD

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    needs: [build-and-test]

    steps:
      - uses: actions/checkout@v4

      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKER_USERNAME }}
          password: ${{ secrets.DOCKER_PASSWORD }}

      - name: Build and Push
        uses: docker/build-push-action@v5
        with:
          context: .
          file: docker/Dockerfile
          push: true
          tags: |
            ${{ secrets.DOCKER_USERNAME }}/englishai-api:latest
            ${{ secrets.DOCKER_USERNAME }}/englishai-api:${{ github.sha }}

      - name: Deploy to Railway
        uses: bervProject/railway-deploy@main
        with:
          railway_token: ${{ secrets.RAILWAY_TOKEN }}
          service: englishai-api
```

---

## Environment Variables

```env
# .env.example

# Database
ConnectionStrings__DefaultConnection=Host=localhost;Database=englishai;Username=postgres;Password=0833114099
ConnectionStrings__Redis=localhost:6379

# JWT
Jwt__Secret=your-very-long-secret-key-at-least-64-chars-for-hmac-sha256-!!
Jwt__Issuer=EnglishAI
Jwt__Audience=EnglishAI
Jwt__AccessTokenExpirationMinutes=15
Jwt__RefreshTokenExpirationDays=7

# Google OAuth
Google__ClientId=your-google-client-id
Google__ClientSecret=your-google-client-secret

# Azure Speech
AzureSpeech__SubscriptionKey=your-azure-speech-key
AzureSpeech__Region=southeastasia

# OpenAI
OpenAI__ApiKey=sk-your-openai-api-key
OpenAI__Model=gpt-4o
OpenAI__MiniModel=gpt-4o-mini

# Storage (MinIO for dev, Azure Blob for prod)
Storage__Provider=MinIO
Storage__ConnectionString=http://localhost:9000
Storage__AccessKey=minioadmin
Storage__SecretKey=minioadmin123
Storage__BucketName=englishai

# Seq Logging
Seq__ServerUrl=http://localhost:5341
Seq__ApiKey=

# App
App__BaseUrl=https://localhost:5001
App__FrontendUrl=http://localhost:3000
App__CorsOrigins=http://localhost:3000,https://englishai.dev
```

---

## Health Checks

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "postgresql")
    .AddRedis(redisConnection, name: "redis")
    .AddUrlGroup(new Uri(seqUrl), name: "seq")
    .AddCheck<AzureSpeechHealthCheck>("azure-speech")
    .AddCheck<OpenAiHealthCheck>("openai");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // just returns 200
});
```

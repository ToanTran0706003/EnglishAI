# Architecture Overview — EnglishAI

## Clean Architecture + CQRS

```
┌─────────────────────────────────────────────────────┐
│                    API Layer                         │
│  Controllers → MediatR Send/Publish → SignalR Hubs  │
├─────────────────────────────────────────────────────┤
│                Application Layer                     │
│  Commands / Queries / Handlers / Validators          │
│  Pipeline Behaviors (Logging, Validation, Caching)   │
├─────────────────────────────────────────────────────┤
│                 Domain Layer                          │
│  Entities, Value Objects, Domain Events, Enums       │
│  NO dependencies on any other layer                  │
├─────────────────────────────────────────────────────┤
│              Infrastructure Layer                     │
│  EF Core, Redis, Azure Speech, OpenAI, Blob Storage │
│  Implements interfaces defined in Application        │
└─────────────────────────────────────────────────────┘
```

## Dependency Rule
- **Domain** → depends on nothing
- **Application** → depends on Domain
- **Infrastructure** → depends on Application + Domain
- **API** → depends on Application (never directly on Infrastructure)

---

## CQRS with MediatR

Every use case is either a **Command** (write) or **Query** (read).

### Command Example
```csharp
// Application/Features/Flashcards/Commands/CreateFlashcard/CreateFlashcardCommand.cs
public record CreateFlashcardCommand : IRequest<FlashcardDto>
{
    public string Front { get; init; } = null!;
    public string Back { get; init; } = null!;
    public Guid DeckId { get; init; }
    public string? ExampleSentence { get; init; }
    public string? AudioUrl { get; init; }
}

// Validator
public class CreateFlashcardCommandValidator : AbstractValidator<CreateFlashcardCommand>
{
    public CreateFlashcardCommandValidator()
    {
        RuleFor(x => x.Front).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Back).NotEmpty().MaximumLength(500);
        RuleFor(x => x.DeckId).NotEmpty();
    }
}

// Handler
public class CreateFlashcardCommandHandler : IRequestHandler<CreateFlashcardCommand, FlashcardDto>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public async Task<FlashcardDto> Handle(CreateFlashcardCommand request, CancellationToken ct)
    {
        var deck = await _context.FlashcardDecks
            .FirstOrDefaultAsync(d => d.Id == request.DeckId && d.UserId == _currentUser.UserId, ct)
            ?? throw new NotFoundException(nameof(FlashcardDeck), request.DeckId);

        var card = new Flashcard
        {
            Front = request.Front,
            Back = request.Back,
            DeckId = deck.Id,
            ExampleSentence = request.ExampleSentence,
            AudioUrl = request.AudioUrl,
            SrsData = SrsData.CreateNew() // Value Object
        };

        _context.Flashcards.Add(card);
        await _context.SaveChangesAsync(ct);
        return _mapper.Map<FlashcardDto>(card);
    }
}
```

### Query Example
```csharp
// Application/Features/Flashcards/Queries/GetDueCards/GetDueCardsQuery.cs
public record GetDueCardsQuery : IRequest<PaginatedList<FlashcardDto>>
{
    public Guid DeckId { get; init; }
    public int PageSize { get; init; } = 20;
}
```

---

## Pipeline Behaviors

Registered in order via MediatR:

```csharp
// 1. Logging — logs every request
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

// 2. Validation — runs FluentValidation before handler
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// 3. Performance — warns if request takes > 500ms
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

// 4. Caching — for Queries only, checks Redis first
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

// 5. Transaction — wraps Commands in DB transaction
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
```

### ValidationBehavior Implementation
```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, ct))))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
            throw new AppValidationException(failures);

        return await next();
    }
}
```

---

## SignalR Hubs

### PronunciationHub
```csharp
public class PronunciationHub : Hub
{
    // Client gửi audio chunk real-time
    public async Task SendAudioChunk(byte[] audioData)
    {
        // Process qua Azure Speech SDK streaming
        // Trả về partial result
        await Clients.Caller.SendAsync("ReceivePartialResult", partialResult);
    }

    // Trả kết quả cuối cùng
    public async Task SendFinalResult(PronunciationResult result)
    {
        await Clients.Caller.SendAsync("ReceiveFinalResult", result);
    }
}
```

### ConversationHub
```csharp
public class ConversationHub : Hub
{
    // AI conversation real-time streaming
    public async Task SendMessage(string message, Guid sessionId)
    {
        // Stream response từ AI
        await foreach (var chunk in _aiService.StreamResponseAsync(message, sessionId))
        {
            await Clients.Caller.SendAsync("ReceiveChunk", chunk);
        }
        await Clients.Caller.SendAsync("ReceiveComplete", sessionId);
    }
}
```

---

## Error Handling Strategy

### Global Exception Handler Middleware
```csharp
public class GlobalExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            var (statusCode, response) = ex switch
            {
                AppValidationException e => (400, new ErrorResponse("Validation", e.Errors)),
                NotFoundException e      => (404, new ErrorResponse("NotFound", e.Message)),
                ForbiddenException       => (403, new ErrorResponse("Forbidden")),
                UnauthorizedException    => (401, new ErrorResponse("Unauthorized")),
                RateLimitException       => (429, new ErrorResponse("TooManyRequests")),
                _                        => (500, new ErrorResponse("InternalError"))
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

### Result Pattern (for complex operations)
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error? Error { get; }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
}
```

---

## Authentication Flow

```
┌──────────┐     ┌──────────┐     ┌──────────┐     ┌──────────┐
│  Client   │────▶│  Login   │────▶│ Validate │────▶│  Issue   │
│           │     │ Endpoint │     │ Password │     │ JWT +    │
│           │◀────│          │◀────│  /OAuth  │◀────│ Refresh  │
└──────────┘     └──────────┘     └──────────┘     └──────────┘

JWT: 15 min expiry, stored in memory (not localStorage)
Refresh Token: 7 day expiry, HTTP-only cookie, rotated on use
```

### JWT Claims
```json
{
  "sub": "user-guid",
  "email": "user@example.com",
  "name": "Nguyen Van A",
  "role": "Learner",
  "level": "B1",
  "iat": 1700000000,
  "exp": 1700000900
}
```

---

## Caching Strategy (Redis)

| Data | Cache Key Pattern | TTL | Invalidation |
|------|------------------|-----|--------------|
| User profile | `user:{id}` | 30m | On update |
| Learning path | `path:{userId}:{level}` | 1h | On lesson complete |
| Leaderboard | `leaderboard:weekly` | 5m | Auto-expire |
| Lesson content | `lesson:{id}` | 24h | On admin edit |
| SRS due cards | `srs:{userId}:due` | 10m | On review |
| AI session context | `ai:session:{id}` | 2h | On session end |

---

## Logging (Serilog + Seq)

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "EnglishAI")
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();
```

Structured logging cho mọi request:
```
[INF] HTTP POST /api/pronunciation/evaluate responded 200 in 1243ms
      UserId: abc-123, AudioDuration: 3.2s, Score: 78.5
```

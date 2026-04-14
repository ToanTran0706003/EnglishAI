using MediatR;

namespace EnglishAI.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Transactions are implemented in Infrastructure (DbContext).
        // This behavior is kept here so handlers can stay unaware of persistence concerns.
        return await next();
    }
}


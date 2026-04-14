using FluentValidation.Results;

namespace EnglishAI.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray(),
                StringComparer.Ordinal);
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}


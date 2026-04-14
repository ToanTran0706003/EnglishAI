using EnglishAI.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EnglishAI.Infrastructure.Persistence.Interceptors;

public sealed class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseAuditableEntity auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedAt = utcNow;
                    auditable.UpdatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditable.UpdatedAt = utcNow;
                }
            }

            if (entry.Entity is ISoftDeletable softDeletable)
            {
                SetDeletedAtIfSoftDeleted(entry, softDeletable, utcNow);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetDeletedAtIfSoftDeleted(
        EntityEntry entry,
        ISoftDeletable softDeletable,
        DateTime utcNow)
    {
        var isDeletedProp = entry.Property(nameof(ISoftDeletable.IsDeleted));
        if (isDeletedProp is null)
        {
            return;
        }

        if (isDeletedProp.IsModified && softDeletable.IsDeleted && softDeletable.DeletedAt is null)
        {
            softDeletable.DeletedAt = utcNow;
        }

        if (isDeletedProp.IsModified && !softDeletable.IsDeleted)
        {
            softDeletable.DeletedAt = null;
        }
    }
}


using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dvchevskii.Blog.Infrastructure.Persistence.Interceptors;

public class EntityAuditInfoSetterInterceptor(
    IAuthenticationContext authenticationContext,
    TimeProvider timeProvider
) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        if (eventData.Context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var currentDateTimeUtc = timeProvider.GetUtcNow().DateTime;

        if (!authenticationContext.IsAuthenticated)
        {
            throw new Exception("Authentication context is not authenticated");
        }

        eventData.Context.ChangeTracker.Entries<EntityBase>()
            .Where(entry => entry.State == EntityState.Added)
            .ToList()
            .ForEach(entry =>
            {
                entry.Entity.EnsureAuditInfoCreated();
                entry.Entity.AuditInfo.CreatedAtUtc = currentDateTimeUtc;
                entry.Entity.AuditInfo.CreatedById = authenticationContext.UserId.Value;
            });

        eventData.Context.ChangeTracker.Entries<EntityBase>()
            .Where(entry => entry.State == EntityState.Modified)
            .ToList()
            .ForEach(entry =>
            {
                entry.Entity.AuditInfo.UpdatedAtUtc = currentDateTimeUtc;
                entry.Entity.AuditInfo.UpdatedById = authenticationContext.UserId.Value;
            });

        eventData.Context.ChangeTracker.Entries<EntityBase>()
            .Where(entry => entry.State == EntityState.Deleted)
            .ToList()
            .ForEach(entry =>
            {
                /*soft-deleting*/
                entry.State = EntityState.Modified;
                entry.Entity.AuditInfo.DeletedAtUtc = currentDateTimeUtc;
                entry.Entity.AuditInfo.DeletedById = authenticationContext.UserId.Value;
            });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

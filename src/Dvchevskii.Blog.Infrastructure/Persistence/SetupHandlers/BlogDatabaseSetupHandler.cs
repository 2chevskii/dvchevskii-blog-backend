using System.Security.Cryptography;
using System.Text;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Infrastructure;
using Dvchevskii.Blog.Core.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dvchevskii.Blog.Infrastructure.Persistence.SetupHandlers;

[SetupUser(nameof(TechnicalUsers.System))]
public class BlogDatabaseSetupHandler(ILogger<BlogDatabaseSetupHandler> logger, BlogDbContext dbContext) : ISetupHandler
{
    public async Task Execute()
    {
        await ApplyMigrations();
        await CreateTechnicalUsers();
    }

    private async ValueTask ApplyMigrations()
    {
        logger.LogInformation("Ensuring database is migrated");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrated OK");
    }

    private async ValueTask CreateTechnicalUsers()
    {
        logger.LogInformation("Creating technical users");

        var systemUser = await dbContext.Users.FindAsync(TechnicalUsers.System.Id);

        if (systemUser == null)
        {
            logger.LogInformation("Creating system user");
            systemUser = new User
            {
                Id = TechnicalUsers.System.Id,
                Username = TechnicalUsers.System.Username,
                IsAdmin = true,
                PasswordHash = [],
            };
            dbContext.Add(systemUser);
        }

        var debugAdminUser = await dbContext.Users.FindAsync(TechnicalUsers.DebugAdmin.Id);

        if (debugAdminUser == null)
        {
            logger.LogInformation("Creating debug admin user");
            debugAdminUser = new User
            {
                Id = TechnicalUsers.DebugAdmin.Id,
                Username = TechnicalUsers.DebugAdmin.Username,
                IsAdmin = true,
                PasswordHash = SHA1.HashData(Encoding.UTF8.GetBytes(nameof(TechnicalUsers.DebugAdmin)))
            };
            dbContext.Add(debugAdminUser);
        }

        var debugUserUser = await dbContext.Users.FindAsync(TechnicalUsers.DebugUser.Id);

        if (debugUserUser == null)
        {
            logger.LogInformation("Creating debug user user");
            debugUserUser = new User
            {
                Id = TechnicalUsers.DebugUser.Id,
                Username = TechnicalUsers.DebugUser.Username,
                IsAdmin = false,
                PasswordHash = SHA1.HashData(Encoding.UTF8.GetBytes(nameof(TechnicalUsers.DebugUser)))
            };
            dbContext.Add(debugUserUser);
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Technical users created OK");
    }
}

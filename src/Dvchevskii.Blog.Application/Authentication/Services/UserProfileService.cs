using Dvchevskii.Blog.Contracts.Authentication.Users;
using Dvchevskii.Blog.Contracts.Files;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Application.Authentication.Services;

internal class UserProfileService(
    BlogDbContext dbContext,
    IImageService imageService
) : IUserProfileService
{
    public async Task<UserProfileDto> UpdateUserProfileAvatar(int userId, int? avatarId)
    {
        var user = await dbContext.Users.FirstAsync(x => x.Id == userId);

        if (avatarId == null)
        {
            user.AvatarId = null;
        }
        else if (!await imageService.Exists(avatarId.Value))
        {
            throw new Exception("Image does not exist");
        }
        else
        {
            user.AvatarId = avatarId.Value;
        }

        await dbContext.SaveChangesAsync();
        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            RegisteredAt = user.AuditInfo.CreatedAtUtc,
            AvatarId = user.AvatarId,
        };
    }

    public async Task<UserProfileDto> Get(int userId)
    {
        var user = await dbContext.Users.FirstAsync(x => x.Id == userId);

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            RegisteredAt = user.AuditInfo.CreatedAtUtc,
            AvatarId = user.AvatarId,
        };
    }
}

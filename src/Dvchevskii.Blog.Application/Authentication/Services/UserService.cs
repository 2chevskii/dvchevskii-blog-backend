using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Contracts.Common;
using Dvchevskii.Blog.Core.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Persistence.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Application.Authentication.Services;

public class UserService(
    IUserPasswordService userPasswordService,
    UserRepository userRepository,
    IUsernameNormalizer usernameNormalizer,
    BlogDbContext dbContext
) : IUserService
{
    public Task<bool> Exists(int id)
    {
        return userRepository.Exists(id);
    }

    public Task<bool> ExistsByUsername(string username)
    {
        return userRepository.ExistsByUsername(usernameNormalizer.Normalize(username));
    }

    public async Task<UserDto> Get(int id)
    {
        var user = await userRepository.Find(id);

        if (user == null)
        {
            throw new Exception();
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            AuditInfo = new AuditInfoDto
            {
                CreatedAtUtc = user.AuditInfo.CreatedAtUtc,
                CreatedById = user.AuditInfo.CreatedById,
                UpdatedAtUtc = user.AuditInfo.UpdatedAtUtc,
                UpdatedById = user.AuditInfo.UpdatedById,
                DeletedAtUtc = user.AuditInfo.DeletedAtUtc,
                DeletedById = user.AuditInfo.DeletedById,
            }
        };
    }

    public async Task<UserDto> GetByUsername(string username)
    {
        var user = await userRepository.FindByUsername(usernameNormalizer.Normalize(username));

        if (user == null)
        {
            throw new Exception();
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            AuditInfo = new AuditInfoDto
            {
                CreatedAtUtc = user.AuditInfo.CreatedAtUtc,
                CreatedById = user.AuditInfo.CreatedById,
                UpdatedAtUtc = user.AuditInfo.UpdatedAtUtc,
                UpdatedById = user.AuditInfo.UpdatedById,
                DeletedAtUtc = user.AuditInfo.DeletedAtUtc,
                DeletedById = user.AuditInfo.DeletedById,
            }
        };
    }

    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var user = new User
        {
            Username = usernameNormalizer.Normalize(dto.Username),
            IsAdmin = dto.IsAdmin,
            PasswordHash = []
        };

        await userRepository.Create(user);
        await userPasswordService.CreatePasswordHash(user.Id, dto.Password);

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            AuditInfo = new AuditInfoDto
            {
                CreatedAtUtc = user.AuditInfo.CreatedAtUtc,
                CreatedById = user.AuditInfo.CreatedById,
                UpdatedAtUtc = user.AuditInfo.UpdatedAtUtc,
                UpdatedById = user.AuditInfo.UpdatedById,
                DeletedAtUtc = user.AuditInfo.DeletedAtUtc,
                DeletedById = user.AuditInfo.DeletedById,
            },
        };
    }

    public Task<List<UserDto>> GetAll(bool includeDeleted = false)
    {
        var query = dbContext.Users.AsQueryable();

        if (!includeDeleted)
        {
            query = query.Where(user => !user.AuditInfo.DeletedAtUtc.HasValue);
        }

        return query.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            AuditInfo = new AuditInfoDto
            {
                CreatedAtUtc = user.AuditInfo.CreatedAtUtc,
                CreatedById = user.AuditInfo.CreatedById,
                UpdatedAtUtc = user.AuditInfo.UpdatedAtUtc,
                UpdatedById = user.AuditInfo.UpdatedById,
                DeletedAtUtc = user.AuditInfo.DeletedAtUtc,
                DeletedById = user.AuditInfo.DeletedById,
            },
        }).ToListAsync();
    }
}

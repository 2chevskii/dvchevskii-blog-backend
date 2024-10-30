using Dvchevskii.Blog.Core.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Persistence.Authentication.Users;

public class UserRepository(BlogDbContext dbContext)
{
    private IQueryable<User> Query => dbContext.Users.AsQueryable().Where(x => !x.AuditInfo.DeletedAtUtc.HasValue);

    private IQueryable<User> QueryDeleted =>
        dbContext.Users.AsQueryable().Where(x => x.AuditInfo.DeletedAtUtc.HasValue);

    public Task<bool> Exists(int id) => Query.AnyAsync(x => x.Id == id);
    public Task<bool> ExistsByUsername(string username) => Query.AnyAsync(x => x.Username == username);

    public Task<User?> Find(int id) => Query.FirstOrDefaultAsync(x => x.Id == id);
    public Task<User?> FindByUsername(string username) => Query.FirstOrDefaultAsync(x => x.Username == username);

    public async Task<User> Create(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}

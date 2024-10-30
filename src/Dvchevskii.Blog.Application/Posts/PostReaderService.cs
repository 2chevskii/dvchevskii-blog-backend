using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Posts;
using Dvchevskii.Blog.Contracts.Posts.Entities;
using Dvchevskii.Blog.Contracts.Posts.Services;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dvchevskii.Blog.Application.Posts;

internal class PostReaderService(BlogDbContext dbContext, IMapper mapper, IAuthenticationContext authenticationContext)
    : IPostReaderService
{
    public Task<PostDisplayInfoDto> GetDisplayInfo(int id)
    {
        return dbContext.Posts
            .Where(post => !post.IsDraft && !post.AuditInfo.DeletedAtUtc.HasValue)
            .ProjectTo<PostDisplayInfoDto>(mapper.ConfigurationProvider)
            .FirstAsync(post => post.Id == id);
    }

    public Task<List<PostDisplayInfoDto>> GetDisplayInfoAll(bool includeDrafts = false, bool includeDeleted = false)
    {
        var query = dbContext.Posts.AsQueryable();

        if (!includeDrafts)
        {
            query = query.Where(post => !post.IsDraft);
        }

        if (!includeDeleted)
        {
            query = query.Where(post => !post.AuditInfo.DeletedAtUtc.HasValue);
        }

        return query
            .ProjectTo<PostDisplayInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public Task<PostDto> Get(int id)
    {
        var query = dbContext.Posts.AsQueryable();
        if (!authenticationContext.IsAdmin)
        {
            query = query.Where(post => !post.IsDraft && !post.AuditInfo.DeletedAtUtc.HasValue);
        }

        return query
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .FirstAsync(post => post.Id == id);
    }
}

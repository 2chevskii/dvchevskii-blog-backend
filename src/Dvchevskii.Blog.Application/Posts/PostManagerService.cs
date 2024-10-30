using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dvchevskii.Blog.Contracts.Posts;
using Dvchevskii.Blog.Contracts.Posts.Entities;
using Dvchevskii.Blog.Contracts.Posts.Services;
using Dvchevskii.Blog.Core.Content.Posts;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Application.Posts;

internal class PostManagerService(BlogDbContext dbContext, IMapper mapper) : IPostManagerService
{
    public Task<PostDto> Get(int id)
    {
        return dbContext.Posts
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .FirstAsync(post => post.Id == id);
    }

    public Task<List<PostDto>> GetAll(bool includeDrafts = false, bool includeDeleted = false)
    {
        return dbContext.Posts
            .Where(post => includeDeleted || !post.AuditInfo.DeletedAtUtc.HasValue)
            .Where(post => includeDrafts || !post.IsDraft)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PostDto> Create(PostDto dto)
    {
        var post = mapper.Map<Post>(dto);
        dbContext.Posts.Add(post);
        await dbContext.SaveChangesAsync();
        return mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> Update(int id, PostDto dto)
    {
        var post = await dbContext.Posts.FirstAsync(x => x.Id == id);
        mapper.Map(dto, post);
        await dbContext.SaveChangesAsync();
        return mapper.Map<PostDto>(post);
    }

    public async Task Delete(int id)
    {
        var post = await dbContext.Posts.FirstAsync(x => x.Id == id);
        dbContext.Remove(post);
        await dbContext.SaveChangesAsync();
    }
}

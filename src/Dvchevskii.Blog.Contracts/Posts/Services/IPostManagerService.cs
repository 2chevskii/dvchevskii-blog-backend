using Dvchevskii.Blog.Contracts.Posts.Entities;

namespace Dvchevskii.Blog.Contracts.Posts.Services;

public interface IPostManagerService
{
    Task<PostDto> Get(int id);

    Task<List<PostDto>> GetAll(bool includeDrafts = false, bool includeDeleted = false);

    Task<PostDto> Create(PostDto dto);

    Task<PostDto> Update(int id, PostDto dto);

    Task Delete(int id);
}

using Dvchevskii.Blog.Contracts.Posts.Entities;

namespace Dvchevskii.Blog.Contracts.Posts.Services;

public interface IPostReaderService
{
    Task<PostDisplayInfoDto> GetDisplayInfo(int id);

    Task<List<PostDisplayInfoDto>> GetDisplayInfoAll(bool includeDrafts = false, bool includeDeleted = false);

    Task<PostDto> Get(int id);
}

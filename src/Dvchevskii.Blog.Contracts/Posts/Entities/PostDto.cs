using Dvchevskii.Blog.Contracts.Common;

namespace Dvchevskii.Blog.Contracts.Posts.Entities;

public class PostDto : EntityBaseDto
{
    public string Title { get; set; }
    public string? Tagline { get; set; }
    public string? Body { get; set; }
    public bool IsDraft { get; set; }
    public int? HeaderImageId { get; set; }
}

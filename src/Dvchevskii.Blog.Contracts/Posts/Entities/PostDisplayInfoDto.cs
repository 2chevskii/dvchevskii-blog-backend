namespace Dvchevskii.Blog.Contracts.Posts.Entities;

public class PostDisplayInfoDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Tagline { get; set; }
    public int? HeaderImageId { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
}

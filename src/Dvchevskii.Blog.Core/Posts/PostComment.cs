using Dvchevskii.Blog.Core.Common;

namespace Dvchevskii.Blog.Core.Posts;

public class PostComment : EntityBase
{
    public string Text { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}

using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Posts;

[ApiController]
[Route("posts/{postId}/comments")]
public class PostCommentsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int postId)
    {
        throw new NotImplementedException();
    }
}

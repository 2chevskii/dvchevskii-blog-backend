using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Posts.Entities;
using Dvchevskii.Blog.Contracts.Posts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Posts;

[ApiController]
[Route("[controller]")]
public class PostsController(
    IPostReaderService postReaderService,
    IPostManagerService postManagerService,
    IAuthenticationContext authenticationContext
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDisplayInfoAll(bool includeDrafts = false, bool includeDeleted = false)
    {
        if (!authenticationContext.IsAdmin && (includeDrafts || includeDeleted))
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                detail: "Include drafts and deleted posts is only available to admins"
            );
        }

        var postDisplayInfoAll = await postReaderService.GetDisplayInfoAll(includeDrafts, includeDeleted);
        return Ok(postDisplayInfoAll);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await postReaderService.Get(id);

        if (!authenticationContext.IsAdmin && (post.IsDraft || post.AuditInfo.DeletedAtUtc.HasValue))
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                detail: "Include drafts and deleted posts is only available to admins"
            );
        }

        return Ok(post);
    }

    [HttpPost, Authorize("admin")]
    public async Task<IActionResult> Create(PostDto dto)
    {
        var post = await postManagerService.Create(dto);

        return Ok(post);
    }

    [HttpPut("{id}"), Authorize("admin")]
    public async Task<IActionResult> Update(int id, PostDto dto)
    {
        var post = await postManagerService.Update(id, dto);
        return Ok(post);
    }

    [HttpDelete("{id}"), Authorize("admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await postManagerService.Delete(id);
        return Ok();
    }
}

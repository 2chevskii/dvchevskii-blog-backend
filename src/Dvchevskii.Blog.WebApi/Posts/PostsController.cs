using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Posts.Entities;
using Dvchevskii.Blog.Contracts.Posts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Posts;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostReaderService postReaderService, IPostManagerService postManagerService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDisplayInfoAll(bool includeDrafts = false, bool includeDeleted = false)
    {
        var postDisplayInfoAll = await postReaderService.GetDisplayInfoAll(includeDrafts, includeDeleted);
        return Ok(postDisplayInfoAll);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await postReaderService.Get(id);
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostDto dto)
    {
        var post = await postManagerService.Create(dto);

        return Ok(post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostDto dto)
    {
        var post = await postManagerService.Update(id, dto);
        return Ok(post);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await postManagerService.Delete(id);
        return Ok();
    }
}

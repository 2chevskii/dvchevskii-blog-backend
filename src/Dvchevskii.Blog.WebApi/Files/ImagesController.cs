using Dvchevskii.Blog.Contracts.Files;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Files;

[ApiController]
[Route("[controller]")]
public class ImagesController(IImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var imageDto = await imageService.Upload(stream, file.FileName);
        return Ok(imageDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(int id)
    {
        var stream = await imageService.Download(id);
        var dto = await imageService.Get(id);
        return File(stream, $"image/{Path.GetExtension(dto.Filename).Replace(".", string.Empty)}");
    }
}

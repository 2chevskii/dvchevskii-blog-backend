using Dvchevskii.Blog.Contracts.Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Auth.Users;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll(true);

        return Ok(users);
    }
}

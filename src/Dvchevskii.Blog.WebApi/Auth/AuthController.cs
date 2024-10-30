using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Dvchevskii.Blog.Application.Authentication;
using Dvchevskii.Blog.Application.Extensions;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dvchevskii.Blog.WebApi.Auth;

[ApiController, Route("[controller]")]
public class AuthController(IBlogAuthenticationService blogAuthenticationService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAuthState()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Unauthorized(new { message = "Authentication failed" });
        }

        var blogClaimsPrincipal = authenticateResult.Principal.AsBlogClaimsPrincipal();

        return Ok(new
        {
            message = "Authenticated successfully",
            blogClaimsPrincipal.UserId,
            blogClaimsPrincipal.Username,
            blogClaimsPrincipal.IsAdmin
        });
    }

    [UseSystemUser]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var result = await blogAuthenticationService.SignUp(mapper.Map<SignUpDto>(request));

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInWithLoginPasswordRequest request)
    {
        var result = await blogAuthenticationService.SignInWithLoginPassword(mapper.Map<LoginPasswordDto>(request));

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        /*
        var principal = BlogClaimsPrincipal.Create(
            result.User.Id,
            result.User.Username,
            result.User.IsAdmin,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        */

        return SignIn(
            result.ClaimsPrincipal!,
            new AuthenticationProperties { },
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }

    [HttpPost, Route("signout")]
    public new IActionResult SignOut()
    {
        return base.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}

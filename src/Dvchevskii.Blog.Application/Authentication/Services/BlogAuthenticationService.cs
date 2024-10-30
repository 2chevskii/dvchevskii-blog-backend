using System.Security.Claims;
using AutoMapper;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Contracts.Authentication.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace Dvchevskii.Blog.Application.Authentication.Services;

public class BlogAuthenticationService(
    IUserService userService,
    IUserPasswordService userPasswordService,
    IOptions<UserSignInOptions> userSignInOptions,
    IMapper mapper
) : IBlogAuthenticationService
{
    public async Task<SignUpResult> SignUp(SignUpDto dto)
    {
        if (await userService.ExistsByUsername(dto.Username))
        {
            return SignUpResult.UsernameExists();
        }

        _ = await userService.Create(mapper.Map<CreateUserDto>(dto));

        return SignUpResult.Ok();
    }

    public async Task<SignInWithLoginPasswordResult> SignInWithLoginPassword(LoginPasswordDto dto)
    {
        UserDto? user = null;

        var loginType = IsEmail(dto.Login) ? LoginType.Email : LoginType.Username;

        SignInWithLoginPasswordResult result;
        if (loginType == LoginType.Email)
        {
            result = await SignInByEmail(dto);
        }
        else
        {
            result = await SignInByUsername(dto);
        }

        return result;
    }

    private async ValueTask<SignInWithLoginPasswordResult> SignInByEmail(LoginPasswordDto dto)
    {
        throw new NotImplementedException("SignIn by email as login is not implemented");
    }

    private async ValueTask<SignInWithLoginPasswordResult> SignInByUsername(LoginPasswordDto dto)
    {
        const LoginType loginType = LoginType.Username;
        if (!await userService.ExistsByUsername(dto.Login))
        {
            return SignInWithLoginPasswordResult.UserNotFound(loginType, dto.Login);
        }

        var user = await userService.GetByUsername(dto.Login);

        if (!IsSignInAllowed(user.Id))
        {
            return SignInWithLoginPasswordResult.UserSignInDisabled(loginType, dto.Login);
        }

        if (!await userPasswordService.VerifyPassword(user.Id, dto.Password))
        {
            return SignInWithLoginPasswordResult.PasswordInvalid(loginType, dto.Login);
        }

        var claimsPrincipal = await CreateClaimsPrincipal(user);

        return SignInWithLoginPasswordResult.Ok(loginType, dto.Login, claimsPrincipal);
    }

    private ValueTask<ClaimsPrincipal> CreateClaimsPrincipal(UserDto user)
    {
        var claimsList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            /*new Claim(ClaimTypes.Email, user.PrimaryEmail),*/
            new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user"),
        };

        var identity = new ClaimsIdentity(
            claimsList,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.NameIdentifier,
            ClaimTypes.Role
        );

        var claimsPrincipal = new ClaimsPrincipal(identity);

        return ValueTask.FromResult(claimsPrincipal);
    }

    private static bool IsEmail(string login)
    {
        return login.Contains('@');
    }

    private bool IsSignInAllowed(int userId)
    {
        return !userSignInOptions.Value.DisabledSignInUserIds.Contains(userId);
    }
}

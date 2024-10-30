using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;

namespace Dvchevskii.Blog.Application.Authentication;

internal class AuthenticationContextFactory : IAuthenticationContextFactory
{
    public IAuthenticationContext CreateDefault() => new AuthenticationContextData();

    public IAuthenticationContext Create(int userId, string username, bool isAdmin) => new AuthenticationContextData
    {
        IsAuthenticated = true,
        UserId = userId,
        Username = username,
        IsAdmin = isAdmin,
    };

    public IAuthenticationContext Create(UserDto? user) => user == null
        ? CreateDefault()
        : new AuthenticationContextData
        {
            IsAuthenticated = true,
            UserId = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
        };
}

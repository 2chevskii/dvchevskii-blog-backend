namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IAuthenticationContextFactory
{
    IAuthenticationContext CreateDefault();
    IAuthenticationContext Create(int userId, string username, bool isAdmin);
    IAuthenticationContext Create(UserDto? user);
}

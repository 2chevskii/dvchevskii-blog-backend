namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IAuthenticationContextProvider
{
    IAuthenticationContext Context { get; }

    IDisposable CreateScope(IAuthenticationContext context);
}

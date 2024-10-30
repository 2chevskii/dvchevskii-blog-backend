using Dvchevskii.Blog.Contracts.Authentication;

namespace Dvchevskii.Blog.Application.Authentication;

internal class AuthenticationContextScope(
    AuthenticationContextProvider authenticationContextProvider
) : IDisposable
{
    private readonly IAuthenticationContext _previous = authenticationContextProvider.Context;
    private bool _disposed;

    public void Dispose()
    {
        CheckDisposed();
        authenticationContextProvider.Context = _previous;
        _disposed = true;
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(AuthenticationContextScope));
        }
    }
}
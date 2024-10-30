using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;

namespace Dvchevskii.Blog.Application.Authentication;

internal class AuthenticationContextProvider(
    IAuthenticationContextFactory authenticationContextFactory
) : IAuthenticationContextProvider, IAuthenticationContext
{
    private static readonly AsyncLocal<AuthenticationContextHolder?> _holder =
        new AsyncLocal<AuthenticationContextHolder?>();

    public bool IsAuthenticated => Context.IsAuthenticated;
    public int? UserId => Context.UserId;
    public string? Username => Context.Username;
    public bool IsAdmin => Context.IsAdmin;
    public string ToDebugString() => Context.ToDebugString();

    public IAuthenticationContext Context
    {
        get
        {
            if (_holder.Value == null)
            {
                _holder.Value = new AuthenticationContextHolder
                {
                    Context = authenticationContextFactory.CreateDefault(),
                };
            }

            return _holder.Value.Context;
        }

        set
        {
            var currentHolder = _holder.Value;

            if (currentHolder != null)
            {
                currentHolder.Context = null!;
            }

            _holder.Value = new AuthenticationContextHolder { Context = value, };
        }
    }

    public IDisposable CreateScope(IAuthenticationContext authenticationContext)
    {
        var scope = new AuthenticationContextScope(this);
        Context = authenticationContext;
        return scope;
    }

    private class AuthenticationContextHolder
    {
        public IAuthenticationContext Context;
    }
}

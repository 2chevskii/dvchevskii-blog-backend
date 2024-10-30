using Dvchevskii.Blog.Contracts.Authentication;

namespace Dvchevskii.Blog.Application.Authentication;

internal class AuthenticationContextData : IAuthenticationContext
{
    public bool IsAuthenticated { get; set; }
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public bool IsAdmin { get; set; }

    public string ToDebugString()
    {
        return $"Authenticated: {IsAuthenticated} ({UserId}, {Username}, {IsAdmin})";
    }
}

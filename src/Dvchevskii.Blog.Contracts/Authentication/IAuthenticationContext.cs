using System.Diagnostics.CodeAnalysis;

namespace Dvchevskii.Blog.Contracts.Authentication;

public interface IAuthenticationContext
{
    [MemberNotNullWhen(true, nameof(UserId))]
    bool IsAuthenticated { get; }

    int? UserId { get; }

    string? Username { get; }
    bool IsAdmin { get; }

    string ToDebugString();
}

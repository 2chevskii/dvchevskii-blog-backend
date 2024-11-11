using System.Security.Claims;
using Dvchevskii.Blog.Contracts.Authentication;

namespace Dvchevskii.Blog.Application.Authentication;

public class BlogClaimsPrincipal(ClaimsPrincipal claimsPrincipal) : ClaimsPrincipal(claimsPrincipal)
{
    public int UserId { get; } = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    public string Username { get; } = claimsPrincipal.FindFirstValue(ClaimTypes.Name)!;

    public bool IsAdmin { get; } = claimsPrincipal.HasClaim(ClaimTypes.Role, "admin");

    public static BlogClaimsPrincipal Create(int userId, string username, bool isAdmin, string authenticationType)
    {
        var claims = new Claim[3]
        {
            new Claim(BlogClaimTypes.UserId, userId.ToString()),
            new Claim(BlogClaimTypes.UserName, username),
            new Claim(BlogClaimTypes.IsAdmin, isAdmin.ToString()),
        };

        var identity = new ClaimsIdentity(claims, authenticationType);

        return new BlogClaimsPrincipal(new ClaimsPrincipal(identity));
    }
}

using Dvchevskii.Blog.Contracts.Authentication.Services;

namespace Dvchevskii.Blog.Application.Authentication.Services;

public class UsernameNormalizer : IUsernameNormalizer
{
    public string Normalize(string username)
    {
        return username.ToLowerInvariant().Trim();
    }
}

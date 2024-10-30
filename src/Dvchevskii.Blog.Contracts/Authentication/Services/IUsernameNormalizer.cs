namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IUsernameNormalizer
{
    string Normalize(string username);
}

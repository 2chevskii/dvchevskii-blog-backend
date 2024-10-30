using Dvchevskii.Blog.Core.Common;

namespace Dvchevskii.Blog.Core.Authentication.Users;

public class User : EntityBase
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool IsAdmin { get; set; }
    public string? PrimaryEmail { get; set; }
}

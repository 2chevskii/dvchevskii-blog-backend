using Dvchevskii.Blog.Contracts.Common;

namespace Dvchevskii.Blog.Contracts.Authentication;

public class UserDto : EntityBaseDto
{
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
}

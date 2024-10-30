namespace Dvchevskii.Blog.Contracts.Authentication;

public static class TechnicalUsers
{
    public static readonly UserDto System = new UserDto { Id = 1, Username = "system", IsAdmin = true, };

    public static readonly UserDto DebugAdmin = new UserDto { Id = 2, Username = "debug_admin", IsAdmin = true, };

    public static readonly UserDto DebugUser = new UserDto { Id = 3, Username = "debug_user", IsAdmin = false };
}

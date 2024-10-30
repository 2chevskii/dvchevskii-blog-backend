namespace Dvchevskii.Blog.Contracts.Authentication.Users;

public class UserSignInOptions
{
    public int[] DisabledSignInUserIds = [TechnicalUsers.System.Id];
}

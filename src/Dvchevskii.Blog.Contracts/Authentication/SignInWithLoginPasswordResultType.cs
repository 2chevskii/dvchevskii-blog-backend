namespace Dvchevskii.Blog.Contracts.Authentication;

public enum SignInWithLoginPasswordResultType
{
    OK,
    USER_NOT_FOUND,
    USER_SIGNIN_DISABLED,
    PASSWORD_INVALID,
}

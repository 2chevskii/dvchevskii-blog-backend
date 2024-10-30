using System.Security.Claims;

namespace Dvchevskii.Blog.Contracts.Authentication;

public class SignInWithLoginPasswordResult
{
    public bool IsSuccess => Type == SignInWithLoginPasswordResultType.OK;

    public LoginType LoginType { get; set; }

    public string AttemptedLogin { get; set; }

    public SignInWithLoginPasswordResultType Type { get; set; }

    public ClaimsPrincipal? ClaimsPrincipal { get; set; }

    public static SignInWithLoginPasswordResult UserNotFound(LoginType loginType, string attemptedLogin)
    {
        return new SignInWithLoginPasswordResult
        {
            LoginType = loginType,
            AttemptedLogin = attemptedLogin,
            Type = SignInWithLoginPasswordResultType.USER_NOT_FOUND,
        };
    }

    public static SignInWithLoginPasswordResult PasswordInvalid(LoginType loginType, string attemptedLogin)
    {
        if (loginType == LoginType.Email)
        {
            /*
             * NOTE:
             * For security reasons, we always return USER_NOT_FOUND
             * when signin was attempted by email used as login
             * This is to not expose correct emails
             * to malicious users
             * Usernames, on the other hand, are public
             */
            return new SignInWithLoginPasswordResult
            {
                LoginType = loginType,
                AttemptedLogin = attemptedLogin,
                Type = SignInWithLoginPasswordResultType.USER_NOT_FOUND,
            };
        }

        return new SignInWithLoginPasswordResult
        {
            LoginType = loginType,
            AttemptedLogin = attemptedLogin,
            Type = SignInWithLoginPasswordResultType.PASSWORD_INVALID,
        };
    }

    public static SignInWithLoginPasswordResult UserSignInDisabled(LoginType loginType, string attemptedLogin)
    {
        return new SignInWithLoginPasswordResult
        {
            LoginType = loginType,
            AttemptedLogin = attemptedLogin,
            Type = SignInWithLoginPasswordResultType.USER_SIGNIN_DISABLED,
        };
    }

    public static SignInWithLoginPasswordResult Ok(
        LoginType loginType,
        string attemptedLogin,
        ClaimsPrincipal claimsPrincipal
    )
    {
        return new SignInWithLoginPasswordResult
        {
            LoginType = loginType,
            AttemptedLogin = attemptedLogin,
            Type = SignInWithLoginPasswordResultType.OK,
            ClaimsPrincipal = claimsPrincipal,
        };
    }
}

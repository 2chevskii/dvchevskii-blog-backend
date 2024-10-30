namespace Dvchevskii.Blog.Contracts.Authentication;

public enum SignUpResultType
{
    Ok,
    UsernameInvalid,
    UsernameExists,
    PasswordInvalid,
}
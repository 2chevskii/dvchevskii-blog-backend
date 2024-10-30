namespace Dvchevskii.Blog.Contracts.Authentication;

public class SignUpResult
{
    public bool IsSuccess => Type == SignUpResultType.Ok;
    public SignUpResultType Type { get; set; }

    public static SignUpResult UsernameExists()
    {
        return new SignUpResult { Type = SignUpResultType.UsernameExists };
    }

    public static SignUpResult Ok()
    {
        return new SignUpResult { Type = SignUpResultType.Ok };
    }
}

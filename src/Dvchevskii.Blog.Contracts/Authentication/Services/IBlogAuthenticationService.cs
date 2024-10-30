namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IBlogAuthenticationService
{
    Task<SignUpResult> SignUp(SignUpDto dto);

    Task<SignInWithLoginPasswordResult> SignInWithLoginPassword(LoginPasswordDto dto);
}

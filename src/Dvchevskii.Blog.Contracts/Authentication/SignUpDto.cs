namespace Dvchevskii.Blog.Contracts.Authentication;

public class SignUpDto
{
    public string Username { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
}

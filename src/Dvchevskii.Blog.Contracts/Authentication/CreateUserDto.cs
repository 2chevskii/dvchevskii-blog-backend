namespace Dvchevskii.Blog.Contracts.Authentication;

public class CreateUserDto
{
    public string Username { get; set; }
    public string? PrimaryEmail { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}

namespace Dvchevskii.Blog.Contracts.Authentication;

public class UserPasswordHashDto
{
    public int UserId { get; set; }
    public byte[] Value { get; set; }
}

namespace Dvchevskii.Blog.Contracts.Authentication.Users;

public interface IUserProfileService
{
    Task<UserProfileDto> UpdateUserProfileAvatar(int userId, int? avatarId);
    Task<UserProfileDto> Get(int userId);
}

public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTimeOffset RegisteredAt { get; set; }
    public int? AvatarId { get; set; }
    public string? AvatarUrl { get; set; }
}

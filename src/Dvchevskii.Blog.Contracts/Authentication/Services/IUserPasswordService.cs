namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IUserPasswordService
{
    Task<UserPasswordHashDto> CreatePasswordHash(int userId, string plainTextPassword);
    Task<UserPasswordHashDto> GetPasswordHash(int userId);
    Task<bool> VerifyPassword(int userId, string plainTextPassword);
}

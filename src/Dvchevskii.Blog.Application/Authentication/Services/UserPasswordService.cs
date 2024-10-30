using System.Security.Cryptography;
using System.Text;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Infrastructure.Persistence.Authentication.Users;

namespace Dvchevskii.Blog.Application.Authentication.Services;

public class UserPasswordService(UserRepository userRepository) : IUserPasswordService
{
    public async Task<UserPasswordHashDto> CreatePasswordHash(int userId, string plainTextPassword)
    {
        var user = await userRepository.Find(userId);
        var hash = Hash(plainTextPassword);
        user.PasswordHash = hash;
        await userRepository.Update(user);
        return new UserPasswordHashDto
        {
            UserId = user.Id,
            Value = user.PasswordHash,
        };
    }

    public async Task<UserPasswordHashDto> GetPasswordHash(int userId)
    {
        var user = await userRepository.Find(userId);

        return new UserPasswordHashDto
        {
            UserId = user.Id,
            Value = user.PasswordHash,
        };
    }

    public async Task<bool> VerifyPassword(int userId, string plainTextPassword)
    {
        var user = await userRepository.Find(userId);
        return Verify(plainTextPassword, user.PasswordHash);
    }

    private static byte[] Hash(string plainText)
    {
        return SHA1.HashData(Encoding.UTF8.GetBytes(plainText));
    }

    private static bool Verify(string plainText, byte[] hash)
    {
        return Hash(plainText).SequenceEqual(hash);
    }
}

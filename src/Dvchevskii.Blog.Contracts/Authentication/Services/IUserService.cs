namespace Dvchevskii.Blog.Contracts.Authentication.Services;

public interface IUserService
{
    Task<bool> Exists(int id);

    Task<bool> ExistsByUsername(string username);

    Task<UserDto> Get(int id);

    Task<UserDto> GetByUsername(string username);

    Task<UserDto> Create(CreateUserDto dto);

    Task<List<UserDto>> GetAll(bool includeDeleted = false);
}

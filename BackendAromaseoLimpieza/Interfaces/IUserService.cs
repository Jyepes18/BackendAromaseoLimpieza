using BackendAromaseoLimpieza.Models.Users;

namespace BackendAromaseoLimpieza.Interfaces;

public interface IUserService
{
    Task<Result<string, int>> CreateUser(User user);
    Task<Result<User, int>> GetUserById(int id);
    Task<Result<string, int>> UpdateUser(User user);
    Task<Result<object, int>> GetUsers(int page, int pageSize, UserFilter filter);
}
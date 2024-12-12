using UserService.Domain.Entity;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(User user);
        Task<List<User>> GetUsersAsync();
        Task UpdateUserRoleAsync(int userId, string newRole);
    }
}

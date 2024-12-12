using UserService.Domain.Entity;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<string> UpdateUserRoleAsync(int userId, string newRole);
    }
}

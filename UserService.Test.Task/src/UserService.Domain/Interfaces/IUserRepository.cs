using UserService.Domain.Entity;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<string> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<string> UpdateUserRoleAsync(int userId, string newRole);
    }
}

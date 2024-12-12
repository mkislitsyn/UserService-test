using UserService.Domain.Entity;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<List<User>> GetUsersAsync();
        Task UpdateUserRoleAsync(int userId, string newRole);
    }
}

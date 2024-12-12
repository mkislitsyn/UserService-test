using UserService.Domain.Entity;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserRoleAsync(int userId, string newRole)
        {
            throw new NotImplementedException();
        }
    }
}

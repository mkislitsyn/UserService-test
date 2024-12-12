using UserService.Application.Interfaces;
using UserService.Domain.Entity;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task CreateUserAsync(User user)
        {
            return _userRepository.CreateUserAsync(user);
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _userRepository.GetUsersAsync();
        }

        public Task UpdateUserRoleAsync(int userId, string newRole)
        {
            return _userRepository.UpdateUserRoleAsync(userId, newRole);
        }
    }
}

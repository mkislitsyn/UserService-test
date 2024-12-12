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

        public async Task<string> CreateUserAsync(User newUser)
        {
            var user = new User
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash),
                Role = newUser.Role
            };            

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<string> UpdateUserRoleAsync(int userId, string newRole)
        {
            return await _userRepository.UpdateUserRoleAsync(userId, newRole);
        }
    }
}

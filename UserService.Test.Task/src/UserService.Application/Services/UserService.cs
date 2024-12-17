using UserService.Application.Dto;
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

        public async Task<string> CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = userDto.Role
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var result = await _userRepository.GetUsersAsync();

            return result;
        }

        public async Task<string> UpdateUserRoleAsync(UserDto userDto)
        {
            var user = new User
            {
                Id = userDto.UserId,
                Role = userDto.Role
            };

            var result = await _userRepository.UpdateUserRoleAsync(user);

            return result;
        }
    }
}

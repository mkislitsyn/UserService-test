using UserService.Application.Dto;
using UserService.Domain.Entity;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserDto request);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<string> UpdateUserRoleAsync(UserDto request);
    }
}

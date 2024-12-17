using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.DbContexts;
using UserService.Domain.Entity;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        internal readonly UserContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserContext userContext, ILogger<UserRepository> logger)
        {
            _logger = logger;
            _context = userContext;
        }

        public async Task<string> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add User Error");

                return $"Errors while addind new user {ex.Message}";
            }
            return $"User {user.Name} was creted successfully";
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.AsNoTracking().ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get users error");

                return [];
            }
        }

        public async Task<string> UpdateUserRoleAsync(User user)
        {
            try
            {
                var foundUser = await _context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);

                if (foundUser == null)
                {
                    return "user dosen't Exist";
                }

                foundUser.Role = user.Role;

                _context.Entry(foundUser).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Update user role error");

                return $"Errors while updating user role for userId {user.Id}: {ex.Message}";
            }

            return $"For user {user.Id} new role {user.Role} was updated successfully";
        }
    }
}
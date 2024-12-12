using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.DbContexts;
using UserService.Domain.Entity;
using UserService.Domain.Enums;
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

                return $"Errors while addind new user {ex}";
            }
            return $"User {user.Name} was creted successfully";
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get users error");

                return [];
            }
        }

        public async Task<string> UpdateUserRoleAsync(int userId, string newRole)
        {
            try
            {
                var user = await _context.Users.Include(p => p.Name).FirstOrDefaultAsync(p => p.Id == userId);

                if (user == null)
                {
                    return "user dosen't Exist";
                }

                if (Enum.TryParse(newRole, out UserRole parsedRole))
                {

                    user.Role = parsedRole;
                    
                    _context.Entry(user).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return $"Role {newRole} dosen't support";
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Update user role error");

                return $"Errors while updating user role for userId {userId}: {ex}";
            }

            return $"For user {userId} new role {newRole} was updated successfully";
        }
    }
}
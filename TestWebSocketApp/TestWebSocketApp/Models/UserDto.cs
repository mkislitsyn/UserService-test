using TestWebSocketApp.Enums;

namespace TestWebSocketApp.Models
{
    public class UserDto
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}

using TestWebSocketApp.Enums;

namespace TestWebSocketApp.Models
{
    public class UserAction
    {
        public ActionType ActionType { get; set; }

        public UserDto User { get; set; }
    }
}

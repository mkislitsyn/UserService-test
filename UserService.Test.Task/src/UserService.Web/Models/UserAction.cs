using UserService.Application.Dto;
using UserService.Web.Enums;

namespace UserService.Web.Models
{
    public class UserAction
    {
        public ActionType ActionType { get; set; }

        public UserDto User {  get; set; }
    }
}

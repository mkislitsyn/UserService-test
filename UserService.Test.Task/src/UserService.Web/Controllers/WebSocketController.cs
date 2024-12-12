using Microsoft.AspNetCore.Mvc;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Application.Validators.Models;
using UserService.Domain.Entity;

namespace UserService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly IUserService _userService;

        public WebSocketController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var validator = new UserValidator();

            var result = validator.Validate(user);

            if (result.IsValid)
            {
                var createResult = await _userService.CreateUserAsync(user);

                return Ok(new { message = createResult });
            }

            var errors = new StringBuilder();
            foreach (var error in result.Errors)
            {
                errors.Append($"{error.ErrorMessage}.{Environment.NewLine}");
            }

            return BadRequest(errors.ToString());
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] User rquest)
        {


            return Ok(new { message = "User updated successfully." });
        }
    }
}

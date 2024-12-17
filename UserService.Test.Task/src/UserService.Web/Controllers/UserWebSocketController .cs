using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using UserService.Application.Dto;
using UserService.Application.Interfaces;
using UserService.Application.Validators.Models;
using UserService.Web.Enums;
using UserService.Web.Models;

namespace UserService.Web.Controllers
{
    [ApiController]
    [Route("ws/[controller]")]
    public class UserWebSocketController : Controller
    {
        private readonly IUserService _userService;

        private readonly CreateUserValidator _createUserValidator;

        private readonly UpdateUserValidator _updateUserValidator;

        public UserWebSocketController(IUserService userService)
        {
            _userService = userService;
            _createUserValidator = new CreateUserValidator();
            _updateUserValidator = new UpdateUserValidator();
        }

        [HttpGet("connect")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                Console.WriteLine("WebSocket connection opened...");

                await HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }

        private async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {receivedMessage}");

                    // Simulate updating user info in a database (just echoing the message for now)
                    //var responseMessage = $"User info updated: {message}";

                    var responseMessage = await ProcessUserAction(receivedMessage);

                    var responseBytes = Encoding.UTF8.GetBytes(responseMessage);

                    await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    Console.WriteLine("WebSocket connection closed!");
                }
            }
        }

        public async Task<string> ProcessUserAction(string message)
        {
            try
            {
                var userAction = JsonSerializer.Deserialize<UserAction>(message);

                if (userAction == null)
                    return "Invalid request format.";

                switch (userAction.ActionType)
                {
                    case ActionType.Create:
                        var newUser = new UserDto
                        {
                            Name = userAction.User.Name,
                            Email = userAction.User.Email,
                            Role = userAction.User.Role,
                            Password = userAction.User.Password
                        };
                        var createValidationResult = _createUserValidator.Validate(newUser);
                        if (createValidationResult.IsValid)
                        {
                            var createUserResult = await _userService.CreateUserAsync(newUser);
                            return createUserResult;
                        }
                        else
                        {
                            return string.Join(Environment.NewLine, createValidationResult.Errors.Select(x => x.ErrorMessage.ToString()));
                        }

                    case ActionType.Update:
                        var updateUser = new UserDto
                        {
                            UserId = userAction.User.UserId,
                            Role = userAction.User.Role
                        };
                        var updateValidationResult = _updateUserValidator.Validate(updateUser);
                        if (updateValidationResult.IsValid)
                        {
                            var updateUserResult = await _userService.UpdateUserRoleAsync(updateUser);
                            return updateUserResult;
                        }
                        else
                        {
                            return string.Join(Environment.NewLine, updateValidationResult.Errors.Select(x => x.ErrorMessage.ToString()));
                        }
                    case ActionType.GetAll:
                        var getAllResult = await _userService.GetUsersAsync();

                        var results = new StringBuilder();

                        results.Append(Environment.NewLine);
                        results.Append("ID\tName\t\tEmail\t\tRole");
                        foreach (var user in getAllResult)
                        {
                            results.Append(Environment.NewLine);
                            results.Append($"{user.Id}\t{user.Name}\t{user.Email}\t{user.Role}");
                        }

                        return results.ToString();

                    default:
                        return "Unknown action.";
                }
            }
            catch (Exception ex)
            {
                return $"Error processing request: {ex.Message}";
            }
        }
    }
}
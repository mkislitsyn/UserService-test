using System.Text.Json;
using Moq;
using UserService.Application.Dto;
using UserService.Application.Interfaces;
using UserService.Domain.Entity;
using UserService.Web.Controllers;
using UserService.Web.Enums;
using UserService.Web.Models;
using Xunit;

namespace UserService.Web.Tests
{
    public class UserWebSocketControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserWebSocketController _controller;

        public UserWebSocketControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserWebSocketController(_mockUserService.Object);
        }

        [Fact]
        public async Task ProcessUserAction_Should_ReturnSuccessMessage_ForValidCreateUser()
        {
            // Arrange
            var userAction = new UserAction
            {
                ActionType = ActionType.Create,
                User = new UserDto
                {
                    Name = "New Name",
                    Email = "new.name@gmail.com",
                    Password = "password!123",
                    Role =  Domain.Enums.UserRole.User
                }
            };

            var message = JsonSerializer.Serialize(userAction);

            _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<UserDto>())).ReturnsAsync("User created successfully");

            // Act
            var result = await _controller.ProcessUserAction(message);

            // Assert
            Assert.Equal("User created successfully", result);
            _mockUserService.Verify(s => s.CreateUserAsync(It.IsAny<UserDto>()), Times.Once);
        }

        [Fact]
        public async Task ProcessUserAction_Should_ReturnValidationErrors_ForInvalidCreateUser()
        {
            // Arrange
            var userAction = new UserAction
            {
                ActionType = ActionType.Create,
                User = new UserDto
                {
                    Name = "", // Invalid name
                    Email = "invalid-email", // Invalid email
                    Password = "",
                    Role = Domain.Enums.UserRole.Unknown
                }
            };

            var message = JsonSerializer.Serialize(userAction);

            // Act
            var result = await _controller.ProcessUserAction(message);

            // Assert
            Assert.Contains("Name is required", result);
            Assert.Contains("Invalid email address", result);
            Assert.Contains("Password is required", result);
            Assert.Contains("Role must be a valid UserRole value", result);
        }

        [Fact]
        public async Task ProcessUserAction_Should_ReturnErrorMessage_ForUnknownActionType()
        {
            // Arrange
            var userAction = new UserAction
            {
                ActionType = (ActionType)999, // Unknown action type
                User = new UserDto()
            };

            var message = JsonSerializer.Serialize(userAction);

            // Act
            var result = await _controller.ProcessUserAction(message);

            // Assert
            Assert.Equal("Unknown action.", result);
        }

        [Fact]
        public async Task ProcessUserAction_Should_ReturnUserList_ForGetAllAction()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "first", Email = "first@example.com", Role = Domain.Enums.UserRole.Admin },
                new User { Id = 2, Name = "second", Email = "second@example.com", Role = Domain.Enums.UserRole.User }
            };

            _mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(users);

            var userAction = new UserAction { ActionType = ActionType.GetAll };
            var message = JsonSerializer.Serialize(userAction);

            // Act
            var result = await _controller.ProcessUserAction(message);

            // Assert
            Assert.Contains("ID\tName", result);
            Assert.Contains("first", result);
            Assert.Contains("second", result);
        }

        [Fact]
        public async Task ProcessUserAction_Should_HandleExceptionsGracefully()
        {
            // Arrange
            var userAction = new UserAction
            {
                ActionType = ActionType.Create,
                User = new UserDto
                {
                    Name = "New Name",
                    Email = "new.name@gmail.com",
                    Password = "password!123",
                    Role = Domain.Enums.UserRole.User
                }
            };

            var message = JsonSerializer.Serialize(userAction);

            _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<UserDto>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.ProcessUserAction(message);

            // Assert
            Assert.Contains("Error processing request: Something went wrong", result);
        }
    }
}

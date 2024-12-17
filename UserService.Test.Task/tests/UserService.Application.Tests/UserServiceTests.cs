using Moq;
using UserService.Application.Dto;
using UserService.Domain.Entity;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Application.Services.UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new Application.Services.UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ReturnsUserId()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "NewName",
                Email = "NewName@gmai.com",
                Password = "password!123",
                Role = UserRole.User
            };

            
            _mockUserRepository
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync("100");

            // Act
            var result = await _userService.CreateUserAsync(userDto);

            // Assert
            Assert.Equal("100", result);
            _mockUserRepository.Verify(
                repo => repo.CreateUserAsync(It.Is<User>(
                    u => u.Name == userDto.Name &&
                         u.Email == userDto.Email &&
                         u.Role == userDto.Role
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task GetUsersAsync_WhenCalled_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "Name_One", Email = "Name_One@gmail.com", Role = UserRole.Admin },
                new User { Id = 2, Name = "Name_Second", Email = "Name_Second@gmail.com", Role = UserRole.User }
            };

            _mockUserRepository
                .Setup(repo => repo.GetUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                user =>
                {
                    Assert.Equal("Name_One", user.Name);
                    Assert.Equal("Admin", user.Role.ToString());
                },
                user =>
                {
                    Assert.Equal("Name_Second", user.Name);
                    Assert.Equal("User", user.Role.ToString());
                });

            _mockUserRepository.Verify(repo => repo.GetUsersAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_ValidUser_ReturnsSuccessMessage()
        {
            // Arrange
            var userDto = new UserDto
            {
                UserId = 1,
                Role = UserRole.Admin
            };

            _mockUserRepository
                .Setup(repo => repo.UpdateUserRoleAsync(It.IsAny<User>()))
                .ReturnsAsync("Success");

            // Act
            var result = await _userService.UpdateUserRoleAsync(userDto);

            // Assert
            Assert.Equal("Success", result);
            _mockUserRepository.Verify(
                repo => repo.UpdateUserRoleAsync(It.Is<User>(
                    u => u.Id == userDto.UserId && u.Role == userDto.Role
                )),
                Times.Once
            );
        }
    }
}

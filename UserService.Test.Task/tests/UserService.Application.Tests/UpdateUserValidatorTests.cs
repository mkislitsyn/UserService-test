using FluentValidation.TestHelper;
using UserService.Application.Dto;
using UserService.Application.Validators.Models;
using UserService.Domain.Enums;
using Xunit;

namespace UserService.Application.Tests
{
    public class UpdateUserValidatorTests
    {
        private readonly UpdateUserValidator _validator;

        public UpdateUserValidatorTests()
        {
            _validator = new UpdateUserValidator();
        }

        [Fact]
        public void Should_HaveError_When_UserIdIsLessThanOrEqualToZero()
        {
            // Arrange
            var user = new UserDto { UserId = 0, Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.UserId)
                  .WithErrorMessage("User id could be greater than 0");
        }

        [Fact]
        public void Should_NotHaveError_When_UserIdIsGreaterThanZero()
        {
            // Arrange
            var user = new UserDto { UserId = 1, Role = UserRole.Admin };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.UserId);
        }

        [Fact]
        public void Should_HaveError_When_RoleIsInvalid()
        {
            // Arrange
            var user = new UserDto { UserId = 1, Role = UserRole.Unknown};

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Role)
                  .WithErrorMessage("Role must be a valid UserRole value (Admin or User).");
        }

        [Fact]
        public void Should_NotHaveError_When_RoleIsValidAdmin()
        {
            // Arrange
            var user = new UserDto { UserId = 1, Role = UserRole.Admin };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Role);
        }

        [Fact]
        public void Should_NotHaveError_When_RoleIsValidUser()
        {
            // Arrange
            var user = new UserDto { UserId = 1, Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Role);
        }
    }
}

using FluentValidation.TestHelper;
using UserService.Application.Dto;
using UserService.Application.Validators.Models;
using UserService.Domain.Enums;
using Xunit;

namespace UserService.Application.Tests
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator;

        public CreateUserValidatorTests()
        {
            _validator = new CreateUserValidator();
        }

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            // Arrange
            var user = new UserDto { Name = "", Email = "emptynName@gmail.com", Password = "password!123", Role = UserRole.User};

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Name)
                  .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Should_HaveError_When_EmailIsEmpty()
        {
            // Arrange
            var user = new UserDto { Name = "Email Empty", Email = "", Password = "password!123", Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email)
                  .WithErrorMessage("Emai is required.");
        }

        [Fact]
        public void Should_HaveError_When_EmailIsInvalid()
        {
            // Arrange
            var user = new UserDto { Name = "Email Invalid", Email = "invalid-email", Password = "password!123", Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email)
                  .WithErrorMessage("Invalid email address.");
        }

        [Fact]
        public void Should_HaveError_When_PasswordIsEmpty()
        {
            // Arrange
            var user = new UserDto { Name = "John Doe", Email = "test@example.com", Password = "", Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Password)
                  .WithErrorMessage("Password is required.");
        }

        [Fact]
        public void Should_HaveError_When_RoleIsInvalid()
        {
            // Arrange
            var user = new UserDto { Name = "John Doe", Email = "test@example.com", Password = "password!123", Role = UserRole.Unknown};

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Role)
                  .WithErrorMessage("Role must be a valid UserRole value (Admin or User).");
        }

        [Fact]
        public void Should_NotHaveError_When_ValidUser()
        {
            // Arrange
            var user = new UserDto { Name = "John Doe", Email = "test@example.com", Password = "password!123", Role = UserRole.User };

            // Act & Assert
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

using FluentValidation;
using UserService.Domain.Entity;
using UserService.Domain.Enums;

namespace UserService.Application.Validators.Models
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Emai is required.").Matches(ValidationRegexPatterns.EmailPattern).WithMessage("Invalid email address.");
            RuleFor(user => user.PasswordHash).NotEmpty().WithMessage("Password is required.");
            RuleFor(user => user.Role).Must(role => role.Equals(UserRole.Admin) || role.Equals(UserRole.User))
                .WithMessage("Role must be either 'Admin' or 'User'.");
        }
    }
}
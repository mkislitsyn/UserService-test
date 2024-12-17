using FluentValidation;
using UserService.Application.Dto;
using UserService.Domain.Enums;

namespace UserService.Application.Validators.Models
{
    public class CreateUserValidator : AbstractValidator<UserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Emai is required.").Matches(ValidationRegexPatterns.EmailPattern).WithMessage("Invalid email address.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(user => user.Role).IsInEnum().WithMessage($"Role must be a valid UserRole value ({UserRole.Admin} or {UserRole.User}).");
        }
    }
}
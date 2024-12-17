using FluentValidation;
using UserService.Application.Dto;
using UserService.Domain.Enums;

namespace UserService.Application.Validators.Models
{
    public class UpdateUserValidator : AbstractValidator<UserDto>
    {
        public UpdateUserValidator()
        { 
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User id could be greater than 0");
            RuleFor(user => user.Role).Must(x => x.Equals(UserRole.Admin) || x.Equals(UserRole.User)).WithMessage($"Role must be a valid UserRole value ({UserRole.Admin} or {UserRole.User}).");
        }
    }
}
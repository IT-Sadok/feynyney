using FluentValidation;
using LogisticsApp.DTO;

namespace LogisticsApp.Application.Validation;

public class RegisterModelValidation : AbstractValidator<RegisterModel>
{
    public RegisterModelValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email format");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}
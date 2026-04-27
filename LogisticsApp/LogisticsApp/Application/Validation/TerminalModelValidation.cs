using System.Data;
using FluentValidation;
using LogisticsApp.DTO;

namespace LogisticsApp.Application.Validation;

public class TerminalModelValidation : AbstractValidator<CreateTerminalRequestModel>
{
    public TerminalModelValidation()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Number is required");
    }
}
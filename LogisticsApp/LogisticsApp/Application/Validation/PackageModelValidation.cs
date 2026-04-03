using FluentValidation;
using LogisticsApp.DTO;

namespace LogisticsApp.Application.Validation;

public class PackageModelValidation : AbstractValidator<CreatePackageRequestModel>
{
    public PackageModelValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0");
        RuleFor(x => x)
            .Must(x => x.OriginTerminalId != x.DestinationTerminalId)
            .WithMessage("Destination terminal id cannot be origin terminal id");
    }
}
using FluentValidation;
using LogisticsApp.DTO;

namespace LogisticsApp.Application.Validation;

public class TransportModelValidation : AbstractValidator<CreateTransportRequestModel>
{
    public TransportModelValidation()
    {
        RuleFor(x => x.TransportType)
            .NotEmpty().WithMessage("Transport type is required");
    }
}
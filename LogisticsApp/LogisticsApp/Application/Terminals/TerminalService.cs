using FluentValidation;
using LogisticsApp.Application.Repositories;
using LogisticsApp.DTO;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Terminals;

public class TerminalService : ITerminalService
{
    private readonly ITerminalRepository _terminalRepository;
    private readonly IValidator<CreateTerminalRequestModel> _validator;

    public TerminalService(
        ITerminalRepository terminalRepository,
        IValidator<CreateTerminalRequestModel> validator)
    {
        _terminalRepository = terminalRepository;
        _validator = validator;
    }

    public async Task<TerminalResponseModel> CreateTerminalAsync(CreateTerminalRequestModel requestModel, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(requestModel, ct);
        
        var terminal = new Terminal
        {
            Number = requestModel.Number,
            Address = requestModel.Address,
        };
        
       await _terminalRepository.AddTerminalAsync(terminal, ct);

       await _terminalRepository.SaveChangesAsync(ct);

       return new TerminalResponseModel(
           terminal.Id,
           terminal.Number,
           terminal.Address
           );
    }
}
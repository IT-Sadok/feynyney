using LogisticsApp.Application.Repositories;
using LogisticsApp.DTO;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Terminals;

public class TerminalService : ITerminalService
{
    private ITerminalRepository _terminalRepository;

    public TerminalService(ITerminalRepository terminalRepository)
    {
        _terminalRepository = terminalRepository;
    }

    public async Task<TerminalResponseModel> CreateTerminalAsync(CreateTerminalRequestModel requestModel, CancellationToken ct)
    {
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
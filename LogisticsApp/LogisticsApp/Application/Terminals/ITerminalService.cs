using LogisticsApp.DTO;

namespace LogisticsApp.Application.Terminals;

public interface ITerminalService
{
    Task<TerminalResponseModel> CreateTerminalAsync(CreateTerminalRequestModel requestModel, CancellationToken ct);
}
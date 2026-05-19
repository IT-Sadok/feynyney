using LogisticsApp.Application.Packages;

namespace LogisticsApp.Workers;

public class TransportAssignmentWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransportAssignmentWorker> _logger;

    public TransportAssignmentWorker(
        IServiceProvider serviceProvider,  
        ILogger<TransportAssignmentWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope =  _serviceProvider.CreateScope();
                
                var packageService = scope.ServiceProvider.GetRequiredService<IPackageService>();
                await packageService.TryAssignWaitingPackagesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transport assignment worker failed");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(45), ct);
        }
    }
}
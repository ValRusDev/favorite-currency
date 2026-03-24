using FavoriteCurrency.CurrencyUpdater.Worker.Configuration;
using FavoriteCurrency.CurrencyUpdater.Worker.Interfaces;
using FavoriteCurrency.CurrencyUpdater.Worker.Services;
using Microsoft.Extensions.Options;

namespace FavoriteCurrency.CurrencyUpdater.Worker;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly CbrOptions _options;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<CbrOptions> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Currency updater worker started");

        await ExecuteIterationSafely(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_options.UpdateIntervalMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ExecuteIterationSafely(stoppingToken);
        }
    }

    private async Task ExecuteIterationSafely(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var client = scope.ServiceProvider.GetRequiredService<ICbrCurrencyClient>();
            var syncService = scope.ServiceProvider.GetRequiredService<ICurrencySyncService>();

            _logger.LogInformation("Starting currency update");

            var xml = await client.GetXmlAsync(cancellationToken);
            var rates = CbrXmlParser.Parse(xml);

            _logger.LogInformation("Parsed {Count} currencies from CBR XML", rates.Count);

            await syncService.SyncAsync(rates, cancellationToken);

            _logger.LogInformation("Currency update finished successfully");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Currency updater worker is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating currencies");
        }
    }
}

using FavoriteCurrency.CurrencyUpdater.Worker.Data;
using FavoriteCurrency.CurrencyUpdater.Worker.Entities;
using FavoriteCurrency.CurrencyUpdater.Worker.Interfaces;
using FavoriteCurrency.CurrencyUpdater.Worker.Models;
using Microsoft.EntityFrameworkCore;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Services
{
    public sealed class CurrencySyncService : ICurrencySyncService
    {
        private readonly IDbContextFactory<CurrencyUpdaterDbContext> _dbContextFactory;
        private readonly ILogger<CurrencySyncService> _logger;

        public CurrencySyncService(
            IDbContextFactory<CurrencyUpdaterDbContext> dbContextFactory,
            ILogger<CurrencySyncService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task SyncAsync(
            IReadOnlyCollection<CbrCurrencyRate> rates,
            CancellationToken cancellationToken)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var existingCurrencies = await dbContext.Currencies
                .ToDictionaryAsync(x => x.Code, cancellationToken);

            var now = DateTime.UtcNow;
            var inserted = 0;
            var updated = 0;

            foreach (var rate in rates)
            {
                if (existingCurrencies.TryGetValue(rate.Code, out var existing))
                {
                    existing.Update(rate.Name, rate.Rate, now);
                    updated++;
                }
                else
                {
                    var currency = new CurrencyRecord(
                        Guid.NewGuid(),
                        rate.Code,
                        rate.Name,
                        rate.Rate,
                        now);

                    await dbContext.Currencies.AddAsync(currency, cancellationToken);
                    inserted++;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Currency sync completed. Total: {Total}, inserted: {Inserted}, updated: {Updated}",
                rates.Count,
                inserted,
                updated);
        }
    }
}

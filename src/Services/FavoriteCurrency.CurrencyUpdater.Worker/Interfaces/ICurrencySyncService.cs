using FavoriteCurrency.CurrencyUpdater.Worker.Models;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Interfaces
{
    public interface ICurrencySyncService
    {
        Task SyncAsync(IReadOnlyCollection<CbrCurrencyRate> rates, CancellationToken cancellationToken);
    }
}

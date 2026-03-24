using FavoriteCurrency.FinanceService.Domain.Entities;

namespace FavoriteCurrency.FinanceService.Application.Abstractions.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Currency>> GetByCodesAsync(
            IReadOnlyCollection<string> codes,
            CancellationToken cancellationToken = default);

        Task AddAsync(Currency currency, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

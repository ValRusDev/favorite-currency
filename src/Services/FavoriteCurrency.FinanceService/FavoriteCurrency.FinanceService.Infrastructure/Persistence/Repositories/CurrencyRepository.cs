using FavoriteCurrency.FinanceService.Application.Abstractions.Repositories;
using FavoriteCurrency.FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FavoriteCurrency.FinanceService.Infrastructure.Persistence.Repositories
{
    public sealed class CurrencyRepository : ICurrencyRepository
    {
        private readonly FinanceDbContext _dbContext;

        public CurrencyRepository(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return _dbContext.Currencies
                .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Currency>> GetByCodesAsync(
            IReadOnlyCollection<string> codes,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Currencies
                .Where(x => codes.Contains(x.Code))
                .ToListAsync(cancellationToken);
        }

        public Task AddAsync(Currency currency, CancellationToken cancellationToken = default)
        {
            return _dbContext.Currencies.AddAsync(currency, cancellationToken).AsTask();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

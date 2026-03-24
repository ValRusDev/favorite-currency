using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using FavoriteCurrency.UserService.Domain.Entities;
using FavoriteCurrency.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FavoriteCurrency.UserService.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .Include(x => x.FavoriteCurrencies)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .Include(x => x.FavoriteCurrencies)
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.AddAsync(user, cancellationToken).AsTask();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

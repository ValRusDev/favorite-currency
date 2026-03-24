using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.UserService.Domain.Entities
{
    public sealed class User
    {
        private readonly List<UserFavoriteCurrency> _favoriteCurrencies = [];

        private User()
        {
        }

        public User(Guid id, string name, string passwordHash)
        {
            Id = id;
            Name = name;
            PasswordHash = passwordHash;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAtUtc { get; private set; }

        public IReadOnlyCollection<UserFavoriteCurrency> FavoriteCurrencies => _favoriteCurrencies.AsReadOnly();

        public void AddFavoriteCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code is required.", nameof(currencyCode));

            var normalizedCode = currencyCode.Trim().ToUpperInvariant();

            if (_favoriteCurrencies.Any(x => x.CurrencyCode == normalizedCode))
                return;

            _favoriteCurrencies.Add(new UserFavoriteCurrency(Id, normalizedCode));
        }

        public void RemoveFavoriteCurrency(string currencyCode)
        {
            var normalizedCode = currencyCode.Trim().ToUpperInvariant();

            var favorite = _favoriteCurrencies.FirstOrDefault(x => x.CurrencyCode == normalizedCode);

            if (favorite is null)
                return;

            _favoriteCurrencies.Remove(favorite);
        }
    }
}

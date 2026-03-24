using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.UserService.Domain.Entities
{
    public sealed class UserFavoriteCurrency
    {
        private UserFavoriteCurrency()
        {
        }

        public UserFavoriteCurrency(Guid userId, string currencyCode)
        {
            UserId = userId;
            CurrencyCode = currencyCode;
        }

        public Guid UserId { get; private set; }
        public string CurrencyCode { get; private set; } = string.Empty;
    }
}

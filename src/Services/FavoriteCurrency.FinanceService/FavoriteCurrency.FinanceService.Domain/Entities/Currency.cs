using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.FinanceService.Domain.Entities
{
    public sealed class Currency
    {
        private Currency()
        {
        }

        public Currency(Guid id, string code, string name, decimal rate)
        {
            Id = id;
            Code = code;
            Name = name;
            Rate = rate;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Rate { get; private set; }
        public DateTime UpdatedAtUtc { get; private set; }

        public void UpdateRate(string name, decimal rate)
        {
            Name = name;
            Rate = rate;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}

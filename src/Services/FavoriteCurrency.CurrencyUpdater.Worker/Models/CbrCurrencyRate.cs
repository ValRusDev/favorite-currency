using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Models
{
    public sealed record CbrCurrencyRate(
        string Code,
        string Name,
        decimal Rate);
}

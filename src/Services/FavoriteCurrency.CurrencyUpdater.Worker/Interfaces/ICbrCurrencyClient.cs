using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Interfaces
{
    public interface ICbrCurrencyClient
    {
        Task<string> GetXmlAsync(CancellationToken cancellationToken);
    }
}

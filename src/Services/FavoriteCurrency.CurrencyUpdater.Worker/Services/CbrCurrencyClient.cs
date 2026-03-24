using FavoriteCurrency.CurrencyUpdater.Worker.Configuration;
using FavoriteCurrency.CurrencyUpdater.Worker.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Services
{
    public sealed class CbrCurrencyClient : ICbrCurrencyClient
    {
        private readonly HttpClient _httpClient;
        private readonly CbrOptions _options;

        public CbrCurrencyClient(HttpClient httpClient, IOptions<CbrOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> GetXmlAsync(CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(_options.Url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}

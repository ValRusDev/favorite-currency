using FavoriteCurrency.CurrencyUpdater.Worker.Models;
using System.Globalization;
using System.Xml.Linq;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Services
{
    public static class CbrXmlParser
    {
        public static IReadOnlyCollection<CbrCurrencyRate> Parse(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentException("XML content is empty.", nameof(xml));

            var document = XDocument.Parse(xml);

            var result = document.Root?
                .Elements("Valute")
                .Select(ParseValute)
                .Where(x => x is not null)
                .Cast<CbrCurrencyRate>()
                .ToList()
                ?? new List<CbrCurrencyRate>();

            return result;
        }

        private static CbrCurrencyRate? ParseValute(XElement element)
        {
            var code = element.Element("CharCode")?.Value?.Trim();
            var name = element.Element("Name")?.Value?.Trim();
            var valueRaw = element.Element("Value")?.Value?.Trim();
            var nominalRaw = element.Element("Nominal")?.Value?.Trim();

            if (string.IsNullOrWhiteSpace(code) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(valueRaw) ||
                string.IsNullOrWhiteSpace(nominalRaw))
            {
                return null;
            }

            if (!TryParseDecimal(valueRaw, out var value))
                return null;

            if (!int.TryParse(nominalRaw, out var nominal) || nominal <= 0)
                return null;

            var rate = decimal.Round(value / nominal, 6, MidpointRounding.AwayFromZero);

            return new CbrCurrencyRate(code, name, rate);
        }

        private static bool TryParseDecimal(string input, out decimal value)
        {
            var normalized = input.Replace(',', '.');

            return decimal.TryParse(
                normalized,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out value);
        }
    }
}

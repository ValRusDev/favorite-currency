namespace FavoriteCurrency.CurrencyUpdater.Worker.Configuration
{
    public sealed class CbrOptions
    {
        public const string SectionName = "CurrencySource";

        public string Url { get; set; } = "http://www.cbr.ru/scripts/XML_daily.asp";
        public int UpdateIntervalMinutes { get; set; } = 60;
    }
}

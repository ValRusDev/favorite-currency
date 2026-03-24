namespace FavoriteCurrency.FinanceService.Application.Features.Rates.GetRatesForUser
{
    public sealed record CurrencyRateDto(
        string Code,
        string Name,
        decimal Rate,
        DateTime UpdatedAtUtc);
}

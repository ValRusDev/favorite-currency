namespace FavoriteCurrency.FinanceService.Application.Models
{
    public sealed record CurrencyRateDto(
        string Code,
        string Name,
        decimal Rate,
        DateTime UpdatedAtUtc);
}

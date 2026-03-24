using FavoriteCurrency.FinanceService.Application.Models;
using MediatR;

namespace FavoriteCurrency.FinanceService.Application.Features.Rates.GetRatesForUser
{
    public sealed record GetRatesForUserQuery(
        Guid UserId,
        string AccessToken) : IRequest<IReadOnlyCollection<CurrencyRateDto>>;
}

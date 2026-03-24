using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.GetFavoriteCurrencies
{
    public sealed record GetFavoriteCurrenciesQuery(Guid UserId)
        : IRequest<IReadOnlyCollection<string>>;
}

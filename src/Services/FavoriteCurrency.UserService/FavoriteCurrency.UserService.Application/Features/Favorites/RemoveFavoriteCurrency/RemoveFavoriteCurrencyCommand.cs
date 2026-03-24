using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.RemoveFavoriteCurrency
{
    public sealed record RemoveFavoriteCurrencyCommand(
        Guid UserId,
        string CurrencyCode) : IRequest;
}

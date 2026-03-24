using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.AddFavoriteCurrency
{
    public sealed record AddFavoriteCurrencyCommand(
        Guid UserId,
        string CurrencyCode) : IRequest;
}

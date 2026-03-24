using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.GetFavoriteCurrencies
{
    public sealed class GetFavoriteCurrenciesQueryHandler
        : IRequestHandler<GetFavoriteCurrenciesQuery, IReadOnlyCollection<string>>
    {
        private readonly IUserRepository _repository;

        public GetFavoriteCurrenciesQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<string>> Handle(
            GetFavoriteCurrenciesQuery request,
            CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(request.UserId, ct)
                ?? throw new Exception("User not found");

            return user.FavoriteCurrencies
                .Select(x => x.CurrencyCode)
                .ToList();
        }
    }
}

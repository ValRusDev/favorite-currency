using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.AddFavoriteCurrency
{
    public sealed class AddFavoriteCurrencyCommandHandler
        : IRequestHandler<AddFavoriteCurrencyCommand>
    {
        private readonly IUserRepository _repository;

        public AddFavoriteCurrencyCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AddFavoriteCurrencyCommand request, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(request.UserId, ct)
                ?? throw new Exception("User not found");

            user.AddFavoriteCurrency(request.CurrencyCode.ToUpper());

            await _repository.SaveChangesAsync(ct);
        }
    }
}

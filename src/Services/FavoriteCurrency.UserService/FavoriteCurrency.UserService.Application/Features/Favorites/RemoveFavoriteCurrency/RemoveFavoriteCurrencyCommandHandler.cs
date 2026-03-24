using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.RemoveFavoriteCurrency
{
    public sealed class RemoveFavoriteCurrencyCommandHandler
        : IRequestHandler<RemoveFavoriteCurrencyCommand>
    {
        private readonly IUserRepository _repository;

        public RemoveFavoriteCurrencyCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(RemoveFavoriteCurrencyCommand request, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(request.UserId, ct)
                ?? throw new Exception("User not found");

            user.RemoveFavoriteCurrency(request.CurrencyCode);

            await _repository.SaveChangesAsync(ct);
        }
    }
}

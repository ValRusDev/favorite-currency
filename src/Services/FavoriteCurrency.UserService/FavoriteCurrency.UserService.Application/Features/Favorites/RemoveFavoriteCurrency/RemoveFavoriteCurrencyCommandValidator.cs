using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteCurrency.UserService.Application.Features.Favorites.RemoveFavoriteCurrency
{
    public sealed class RemoveFavoriteCurrencyCommandValidator : AbstractValidator<RemoveFavoriteCurrencyCommand>
    {
        public RemoveFavoriteCurrencyCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.CurrencyCode)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}

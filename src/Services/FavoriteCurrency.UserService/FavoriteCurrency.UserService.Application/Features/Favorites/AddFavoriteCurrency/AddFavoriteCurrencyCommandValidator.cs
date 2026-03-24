using FluentValidation;
    
namespace FavoriteCurrency.UserService.Application.Features.Favorites.AddFavoriteCurrency
{
    public sealed class AddFavoriteCurrencyCommandValidator : AbstractValidator<AddFavoriteCurrencyCommand>
    {
        public AddFavoriteCurrencyCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.CurrencyCode)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}

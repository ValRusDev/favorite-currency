using FavoriteCurrency.UserService.Domain.Entities;
using FluentAssertions;

namespace FavoriteCurrency.UserService.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void AddFavoriteCurrency_Should_Normalize_Code_And_Not_Add_Duplicates()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");

            user.AddFavoriteCurrency(" usd ");
            user.AddFavoriteCurrency("USD");

            user.FavoriteCurrencies.Should().ContainSingle();
            user.FavoriteCurrencies.Single().CurrencyCode.Should().Be("USD");
        }

        [Fact]
        public void RemoveFavoriteCurrency_Should_Remove_Existing_Code()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");
            user.AddFavoriteCurrency("USD");
            user.AddFavoriteCurrency("EUR");

            user.RemoveFavoriteCurrency(" usd ");

            user.FavoriteCurrencies.Should().ContainSingle(x => x.CurrencyCode == "EUR");
        }

        [Fact]
        public void AddFavoriteCurrency_Should_Throw_When_Code_Is_Empty()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");

            var act = () => user.AddFavoriteCurrency(" ");

            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Currency code is required.*");
        }
    }
}

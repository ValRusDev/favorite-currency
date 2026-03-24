using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using FavoriteCurrency.UserService.Application.Features.Favorites.AddFavoriteCurrency;
using FavoriteCurrency.UserService.Application.Features.Favorites.GetFavoriteCurrencies;
using FavoriteCurrency.UserService.Application.Features.Favorites.RemoveFavoriteCurrency;
using FavoriteCurrency.UserService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FavoriteCurrency.UserService.UnitTests
{
    public class FavoriteCurrencyHandlersTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        [Fact]
        public async Task AddFavoriteCurrency_Should_Add_Normalized_Code_And_Save()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var handler = new AddFavoriteCurrencyCommandHandler(_userRepositoryMock.Object);

            await handler.Handle(
                new AddFavoriteCurrencyCommand(user.Id, " usd "),
                CancellationToken.None);

            user.FavoriteCurrencies.Should().ContainSingle(x => x.CurrencyCode == "USD");

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFavoriteCurrencies_Should_Return_User_Favorites()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");
            user.AddFavoriteCurrency("usd");
            user.AddFavoriteCurrency("eur");

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var handler = new GetFavoriteCurrenciesQueryHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(
                new GetFavoriteCurrenciesQuery(user.Id),
                CancellationToken.None);

            result.Should().BeEquivalentTo(["USD", "EUR"]);
        }

        [Fact]
        public async Task RemoveFavoriteCurrency_Should_Remove_Code_And_Save()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hash");
            user.AddFavoriteCurrency("USD");
            user.AddFavoriteCurrency("EUR");

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var handler = new RemoveFavoriteCurrencyCommandHandler(_userRepositoryMock.Object);

            await handler.Handle(
                new RemoveFavoriteCurrencyCommand(user.Id, " usd "),
                CancellationToken.None);

            user.FavoriteCurrencies.Should().ContainSingle(x => x.CurrencyCode == "EUR");

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using FavoriteCurrency.FinanceService.Application.Abstractions.External;
using FavoriteCurrency.FinanceService.Application.Abstractions.Repositories;
using FavoriteCurrency.FinanceService.Application.Features.Rates.GetRatesForUser;
using FavoriteCurrency.FinanceService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FavoriteCurrency.FinanceService.UnitTests
{
    public class GetRatesForUserQueryHandlerTests
    {
        private readonly Mock<IUserServiceClient> _userServiceClientMock = new();
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock = new();

        [Fact]
        public async Task Handle_Should_Return_Empty_When_User_Has_No_Favorites()
        {
            var userId = Guid.NewGuid();

            _userServiceClientMock
                .Setup(x => x.GetFavoritesAsync(userId, "token", It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<string>());

            var handler = new GetRatesForUserQueryHandler(
                _userServiceClientMock.Object,
                _currencyRepositoryMock.Object);

            var result = await handler.Handle(
                new GetRatesForUserQuery(userId, "token"),
                CancellationToken.None);

            result.Should().BeEmpty();

            _currencyRepositoryMock.Verify(
                x => x.GetByCodesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Normalize_Distinct_Codes_And_Return_Sorted_Rates()
        {
            var userId = Guid.NewGuid();

            _userServiceClientMock
                .Setup(x => x.GetFavoritesAsync(userId, "token", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { " usd ", "EUR", "usd", " " });

            var usd = new Currency(Guid.NewGuid(), "USD", "US Dollar", 90.12m);
            var eur = new Currency(Guid.NewGuid(), "EUR", "Euro", 98.34m);

            _currencyRepositoryMock
                .Setup(x => x.GetByCodesAsync(
                    It.Is<IReadOnlyCollection<string>>(codes =>
                        codes.Count == 2 &&
                        codes.Contains("USD") &&
                        codes.Contains("EUR")),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { usd, eur });

            var handler = new GetRatesForUserQueryHandler(
                _userServiceClientMock.Object,
                _currencyRepositoryMock.Object);

            var result = await handler.Handle(
                new GetRatesForUserQuery(userId, "token"),
                CancellationToken.None);

            result.Should().HaveCount(2);
            result.Select(x => x.Code).Should().Equal("EUR", "USD");
            result.Should().Contain(x => x.Code == "USD" && x.Name == "US Dollar" && x.Rate == 90.12m);
            result.Should().Contain(x => x.Code == "EUR" && x.Name == "Euro" && x.Rate == 98.34m);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_When_All_Favorites_Are_Invalid()
        {
            var userId = Guid.NewGuid();

            _userServiceClientMock
                .Setup(x => x.GetFavoritesAsync(userId, "token", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { "", " ", "   " });

            var handler = new GetRatesForUserQueryHandler(
                _userServiceClientMock.Object,
                _currencyRepositoryMock.Object);

            var result = await handler.Handle(
                new GetRatesForUserQuery(userId, "token"),
                CancellationToken.None);

            result.Should().BeEmpty();

            _currencyRepositoryMock.Verify(
                x => x.GetByCodesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}

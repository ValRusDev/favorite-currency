using FavoriteCurrency.UserService.Application.Abstractions.Authentication;
using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using FavoriteCurrency.UserService.Application.Features.Auth.LoginUser;
using FavoriteCurrency.UserService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FavoriteCurrency.UserService.UnitTests
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IJwtTokenProvider> _jwtTokenProviderMock = new();

        [Fact]
        public async Task Handle_Should_Return_Token_When_Credentials_Are_Valid()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hashed-password");
            var expiresAt = DateTime.UtcNow.AddHours(1);

            _userRepositoryMock
                .Setup(x => x.GetByNameAsync("ValRus", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(x => x.Verify("Qwerty123!", "hashed-password"))
                .Returns(true);

            _jwtTokenProviderMock
                .Setup(x => x.Generate(user))
                .Returns(("jwt-token", expiresAt));

            var handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenProviderMock.Object);

            var result = await handler.Handle(
                new LoginUserCommand("ValRus", "Qwerty123!"),
                CancellationToken.None);

            result.AccessToken.Should().Be("jwt-token");
            result.ExpiresAtUtc.Should().Be(expiresAt);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_User_Not_Found()
        {
            _userRepositoryMock
                .Setup(x => x.GetByNameAsync("ValRus", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenProviderMock.Object);

            var act = async () => await handler.Handle(
                new LoginUserCommand("ValRus", "Qwerty123!"),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials.");
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Password_Is_Invalid()
        {
            var user = new User(Guid.NewGuid(), "ValRus", "hashed-password");

            _userRepositoryMock
                .Setup(x => x.GetByNameAsync("ValRus", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(x => x.Verify("wrong-password", "hashed-password"))
                .Returns(false);

            var handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenProviderMock.Object);

            var act = async () => await handler.Handle(
                new LoginUserCommand("ValRus", "wrong-password"),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials.");
        }
    }
}

using FavoriteCurrency.UserService.Application.Abstractions.Authentication;
using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using FavoriteCurrency.UserService.Application.Features.Auth.RegisterUser;
using FavoriteCurrency.UserService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FavoriteCurrency.UserService.UnitTests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

        [Fact]
        public async Task Handle_Should_Create_User_And_Save_When_Name_Is_Unique()
        {
            var command = new RegisterUserCommand(" ValRus ", "Qwerty123!");

            _userRepositoryMock
                .Setup(x => x.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordHasherMock
                .Setup(x => x.Hash(command.Password))
                .Returns("hashed-password");

            var handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();

            _passwordHasherMock.Verify(x => x.Hash(command.Password), Times.Once);

            _userRepositoryMock.Verify(x => x.AddAsync(
                    It.Is<User>(u =>
                        u.Id == result &&
                        u.Name == "ValRus" &&
                        u.PasswordHash == "hashed-password"),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_User_Already_Exists()
        {
            var command = new RegisterUserCommand("ValRus", "Qwerty123!");

            _userRepositoryMock
                .Setup(x => x.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object);

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("User with this name already exists.");

            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

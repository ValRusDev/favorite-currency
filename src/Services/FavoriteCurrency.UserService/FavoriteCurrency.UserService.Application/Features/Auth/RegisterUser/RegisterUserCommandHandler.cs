using FavoriteCurrency.UserService.Application.Abstractions.Authentication;
using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using FavoriteCurrency.UserService.Domain.Entities;
using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.RegisterUser
{
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (exists)
                throw new InvalidOperationException("User with this name already exists.");

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(Guid.NewGuid(), request.Name.Trim(), passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}

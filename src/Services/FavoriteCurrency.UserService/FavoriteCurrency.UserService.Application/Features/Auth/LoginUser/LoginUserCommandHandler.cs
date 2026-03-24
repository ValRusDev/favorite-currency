using FavoriteCurrency.UserService.Application.Abstractions.Authentication;
using FavoriteCurrency.UserService.Application.Abstractions.Repositories;
using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.LoginUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenProvider jwtTokenProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByNameAsync(request.Name, cancellationToken);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var isValidPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var (token, expiresAtUtc) = _jwtTokenProvider.Generate(user);

            return new LoginUserResponse(token, expiresAtUtc);
        }
    }
}

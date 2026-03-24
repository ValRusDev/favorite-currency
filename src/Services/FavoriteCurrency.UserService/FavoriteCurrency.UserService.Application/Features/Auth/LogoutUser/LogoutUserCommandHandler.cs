using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.LogoutUser
{
    public sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        public Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

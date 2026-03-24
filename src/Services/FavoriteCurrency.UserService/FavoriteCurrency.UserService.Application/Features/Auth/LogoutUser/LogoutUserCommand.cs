using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.LogoutUser
{
    public sealed record LogoutUserCommand(Guid UserId) : IRequest;
}

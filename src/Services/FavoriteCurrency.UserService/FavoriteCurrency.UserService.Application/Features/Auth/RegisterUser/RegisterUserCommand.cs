using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Name,
        string Password) : IRequest<Guid>;
}

using MediatR;

namespace FavoriteCurrency.UserService.Application.Features.Auth.LoginUser
{
    public sealed record LoginUserCommand(
        string Name,
        string Password) : IRequest<LoginUserResponse>;
}

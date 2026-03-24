using FavoriteCurrency.UserService.Domain.Entities;

namespace FavoriteCurrency.UserService.Application.Abstractions.Authentication
{
    public interface IJwtTokenProvider
    {
        (string Token, DateTime ExpiresAtUtc) Generate(User user);
    }
}

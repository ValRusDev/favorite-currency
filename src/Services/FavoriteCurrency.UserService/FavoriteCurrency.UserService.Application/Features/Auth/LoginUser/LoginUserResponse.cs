namespace FavoriteCurrency.UserService.Application.Features.Auth.LoginUser
{
    public sealed record LoginUserResponse(
        string AccessToken,
        DateTime ExpiresAtUtc);
}

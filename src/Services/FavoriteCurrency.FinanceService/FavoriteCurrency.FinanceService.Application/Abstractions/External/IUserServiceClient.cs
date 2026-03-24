namespace FavoriteCurrency.FinanceService.Application.Abstractions.External
{
    public interface IUserServiceClient
    {
        Task<IReadOnlyCollection<string>> GetFavoritesAsync(
            Guid userId,
            string accessToken,
            CancellationToken cancellationToken = default);
    }
}

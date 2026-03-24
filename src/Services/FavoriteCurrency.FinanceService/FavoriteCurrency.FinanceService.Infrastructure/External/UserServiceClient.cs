using FavoriteCurrency.FinanceService.Application.Abstractions.External;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FavoriteCurrency.FinanceService.Infrastructure.External
{
    public sealed class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<string>> GetFavoritesAsync(
            Guid userId,
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/users/favorites");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var favorites = await response.Content.ReadFromJsonAsync<List<string>>(cancellationToken: cancellationToken);

            return favorites ?? [];
        }
    }
}

using FavoriteCurrency.FinanceService.Application.Abstractions.External;
using FavoriteCurrency.FinanceService.Application.Abstractions.Repositories;
using FavoriteCurrency.FinanceService.Application.Models;
using MediatR;

namespace FavoriteCurrency.FinanceService.Application.Features.Rates.GetRatesForUser
{
    public sealed class GetRatesForUserQueryHandler
        : IRequestHandler<GetRatesForUserQuery, IReadOnlyCollection<CurrencyRateDto>>
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly ICurrencyRepository _currencyRepository;

        public GetRatesForUserQueryHandler(
            IUserServiceClient userServiceClient,
            ICurrencyRepository currencyRepository)
        {
            _userServiceClient = userServiceClient;
            _currencyRepository = currencyRepository;
        }

        public async Task<IReadOnlyCollection<CurrencyRateDto>> Handle(
            GetRatesForUserQuery request,
            CancellationToken cancellationToken)
        {
            var favoriteCodes = await _userServiceClient.GetFavoritesAsync(
                request.UserId,
                request.AccessToken,
                cancellationToken);

            if (favoriteCodes.Count == 0)
                return Array.Empty<CurrencyRateDto>();

            var normalizedCodes = favoriteCodes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .Distinct()
                .ToArray();

            if (normalizedCodes.Length == 0)
                return Array.Empty<CurrencyRateDto>();

            var currencies = await _currencyRepository.GetByCodesAsync(normalizedCodes, cancellationToken);

            return currencies
                .OrderBy(x => x.Code)
                .Select(x => new CurrencyRateDto(
                    x.Code,
                    x.Name,
                    x.Rate,
                    x.UpdatedAtUtc))
                .ToArray();
        }
    }
}

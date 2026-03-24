using FavoriteCurrency.FinanceService.Application.Features.Rates.GetRatesForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FavoriteCurrency.FinanceService.API.Controllers
{
    [ApiController]
    [Route("api/finance/rates")]
    [Authorize]
    public sealed class RatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CurrencyRateDto>>> Get(CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var authorizationHeader = HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authorizationHeader) ||
                !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            var accessToken = authorizationHeader["Bearer ".Length..].Trim();

            var result = await _mediator.Send(
                new GetRatesForUserQuery(userId, accessToken),
                cancellationToken);

            return Ok(result);
        }
    }
}

using FavoriteCurrency.UserService.Application.Features.Favorites.AddFavoriteCurrency;
using FavoriteCurrency.UserService.Application.Features.Favorites.GetFavoriteCurrencies;
using FavoriteCurrency.UserService.Application.Features.Favorites.RemoveFavoriteCurrency;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FavoriteCurrency.UserService.API.Controllers
{
    [ApiController]
    [Route("api/users/favorites")]
    [Authorize]
    public sealed class FavoritesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string code, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _mediator.Send(new AddFavoriteCurrencyCommand(userId, code), ct);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(string code, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _mediator.Send(new RemoveFavoriteCurrencyCommand(userId, code), ct);

            return NoContent();
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<string>> Get(CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return await _mediator.Send(new GetFavoriteCurrenciesQuery(userId), ct);
        }
    }
}

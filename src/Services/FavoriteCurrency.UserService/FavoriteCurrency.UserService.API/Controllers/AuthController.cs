using FavoriteCurrency.UserService.Application.Features.Auth.LoginUser;
using FavoriteCurrency.UserService.Application.Features.Auth.LogoutUser;
using FavoriteCurrency.UserService.Application.Features.Auth.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FavoriteCurrency.UserService.API.Controllers
{
    [ApiController]
    [Route("api/users/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Guid>> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return Ok(userId);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginUserResponse>> Login(
            [FromBody] LoginUserCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            await _mediator.Send(new LogoutUserCommand(userId), cancellationToken);
            return NoContent();
        }
    }
}

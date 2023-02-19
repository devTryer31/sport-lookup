using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportLookup.Backend.UseCases.Auth.Commands.RegisterUser;
using SportLookup.Backend.UseCases.Auth.DTOs;

namespace SportLookup.Backend.WebAPI.Controllers;

[ApiController, AllowAnonymous]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Регитсрация пользователя в системе.
    /// </summary>
    /// <returns>Токен авторизации</returns>
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        bool gisterResult = await _mediator.Send(new RegisterUserCommandRequest { User = registerUserDTO });

        if (!gisterResult)
            return StatusCode(505, "User cannot register by entered params");
        //TODO: Add user-friendly errors explanation.
        //TODO: Add global exceptions handler.

        return Ok($"{registerUserDTO.UserName}'s registration successful!");
    }
}

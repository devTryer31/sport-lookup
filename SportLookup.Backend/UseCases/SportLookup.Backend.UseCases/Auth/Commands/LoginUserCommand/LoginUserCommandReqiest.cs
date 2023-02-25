using MediatR;
using SportLookup.Backend.UseCases.Auth.DTOs;

namespace SportLookup.Backend.UseCases.Auth.Commands.LoginUserCommand;

public class LoginUserCommandReqiest : IRequest<string>
{
    public LoginUserDTO loginUserDTO { get; set; } = null!;
}

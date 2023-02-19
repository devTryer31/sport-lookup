using MediatR;
using SportLookup.Backend.UseCases.Auth.DTOs;

namespace SportLookup.Backend.UseCases.Auth.Commands.RegisterUser;

public class RegisterUserCommandRequest : IRequest<bool>
{
    public RegisterUserDTO User { get; set; } = null!;
}

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SportLookup.Backend.Entities.Configuration;
using SportLookup.Backend.Entities.Exceptions;
using SportLookup.Backend.Entities.Models.Auth;
using SportLookup.Backend.UseCases.Auth.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SportLookup.Backend.UseCases.Auth.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandReqiest, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JWTConfig _jwtConfig;

    public LoginUserCommandHandler(UserManager<AppUser> userManager, JWTConfig jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> Handle(LoginUserCommandReqiest request, CancellationToken cancellationToken)
    {
        LoginUserDTO loginContext = request.loginUserDTO;

        AppUser? user = await _userManager.FindByNameAsync(loginContext.UserName);

        if (user is null)
            throw new UserNotFoundException(loginContext.UserName);

        var passCheckSuccess = await _userManager.CheckPasswordAsync(user, loginContext.Password);

        if (!passCheckSuccess)
            throw new UserNotFoundException(msg: "Password incorrect", loginContext.UserName);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddSeconds(new Random().Next(3, 7 + 1)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtConfig.Secret), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler handler = new();
        var token = handler.CreateJwtSecurityToken(tokenDescriptor);

        return handler.WriteToken(token);
    }
}

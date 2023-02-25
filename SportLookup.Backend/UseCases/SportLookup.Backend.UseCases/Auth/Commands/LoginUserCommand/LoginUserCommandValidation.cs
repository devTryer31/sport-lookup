using FluentValidation;

namespace SportLookup.Backend.UseCases.Auth.Commands.LoginUserCommand;

public class LoginUserCommandValidation : AbstractValidator<LoginUserCommandReqiest>
{
	public LoginUserCommandValidation()
	{
        const string errSuffix = $" at {nameof(LoginUserCommandReqiest)}";

        RuleFor(req => req.loginUserDTO).NotNull()
            .WithMessage("User DTO must exist" + errSuffix);
        RuleFor(req => req.loginUserDTO!.UserName).NotNull().NotEmpty()
            .WithMessage("Username cannot be null or empty" + errSuffix);
        RuleFor(req => req.loginUserDTO!.Password).NotNull().NotEmpty()
            .WithMessage("User password cannot be null or empty" + errSuffix);
    }
}

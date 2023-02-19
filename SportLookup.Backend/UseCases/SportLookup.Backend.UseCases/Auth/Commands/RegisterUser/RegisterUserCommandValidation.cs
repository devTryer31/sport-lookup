using FluentValidation;

namespace SportLookup.Backend.UseCases.Auth.Commands.RegisterUser;

public class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandValidation()
    {
        const string errSuffix = $" at {nameof(RegisterUserCommandRequest)}";

        RuleFor(req => req.User).NotNull()
            .WithMessage("User DTO must exist" + errSuffix);
        RuleFor(req => req.User!.UserName).NotNull().NotEmpty()
            .WithMessage("Username cannot be null or empty" + errSuffix);
        RuleFor(req => req.User!.Password).NotNull().NotEmpty()
            .WithMessage("User password cannot be null or empty" + errSuffix);
        RuleFor(req => req.User!.EMail).NotNull().NotEmpty()
            .WithMessage("Email cannot be null or empty" + errSuffix);
    }
}

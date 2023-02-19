using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SportLookup.Backend.Entities.Models.Auth;

namespace SportLookup.Backend.UseCases.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppUserRole> _roleManager;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppUserRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
    {
        AppUser user = _mapper.Map<AppUser>(request.User);

        var isFirstUser = !_userManager.Users.Any();

        var registerResult = await _userManager.CreateAsync(user, request.User.Password);

        if(!registerResult.Succeeded)
            return false;

        if(isFirstUser)//If it the first user, we create Admin role and register user like Admin.
        {
            var roleCreationResult = await _roleManager.CreateAsync(new AppUserRole { Name = "Admin" });
            if(!roleCreationResult.Succeeded)
                throw new InvalidOperationException("Cannot create Admin role");
            var addAdminResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!addAdminResult.Succeeded)
                throw new InvalidOperationException($"Cannot register user {user.UserName} like a Admin");
        }

        return true;
    }
}

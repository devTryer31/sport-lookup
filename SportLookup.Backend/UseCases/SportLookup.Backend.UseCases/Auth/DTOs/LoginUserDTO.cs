namespace SportLookup.Backend.UseCases.Auth.DTOs;

public class LoginUserDTO
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}

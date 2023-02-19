using System.ComponentModel.DataAnnotations;

namespace SportLookup.Backend.UseCases.Auth.DTOs;

public class RegisterUserDTO
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string EMail { get; set; } = null!;
}

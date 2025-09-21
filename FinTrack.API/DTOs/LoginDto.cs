using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}
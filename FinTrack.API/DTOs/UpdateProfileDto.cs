namespace FinTrack.API.DTOs;

public class UpdateProfileDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
}
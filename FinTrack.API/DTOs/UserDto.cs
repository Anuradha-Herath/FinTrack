namespace FinTrack.API.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? ProfilePicture { get; set; }
}
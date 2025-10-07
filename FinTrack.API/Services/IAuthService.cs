using FinTrack.API.DTOs;

namespace FinTrack.API.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);
    Task<AuthResult> LoginAsync(LoginDto dto);
    Task<UserDto?> GetUserProfileAsync(int userId);
    Task<AuthResult> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    Task<AuthResult> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    Task<AuthResult> UpdateProfilePictureAsync(int userId, string profilePicturePath);
}
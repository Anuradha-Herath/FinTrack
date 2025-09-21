using FinTrack.API.DTOs;

namespace FinTrack.API.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);
    Task<AuthResult> LoginAsync(LoginDto dto);
}
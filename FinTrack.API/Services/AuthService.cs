using FinTrack.API.DTOs;
using FinTrack.API.Models;
using FinTrack.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack.API.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto dto)
    {
        var users = await _userRepository.GetAllAsync();
        var existingUser = users.FirstOrDefault(u => u.Email == dto.Email);
        if (existingUser != null)
        {
            return new AuthResult { Success = false, Message = "User already exists" };
        }

        var user = new User
        {
            Name = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResult
        {
            Success = true,
            Token = token,
            User = new UserDto { UserId = user.UserId, Name = user.Name, Email = user.Email }
        };
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return new AuthResult { Success = false, Message = "Invalid credentials" };
        }

        var token = GenerateJwtToken(user);

        return new AuthResult
        {
            Success = true,
            Token = token,
            User = new UserDto { UserId = user.UserId, Name = user.Name, Email = user.Email }
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UserDto?> GetUserProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            ProfilePicture = user.ProfilePicture
        };
    }

    public async Task<AuthResult> UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Success = false, Message = "User not found" };
        }

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Phone = dto.Phone;

        await _userRepository.SaveChangesAsync();

        return new AuthResult { Success = true, Message = "Profile updated successfully" };
    }

    public async Task<AuthResult> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Success = false, Message = "User not found" };
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
        {
            return new AuthResult { Success = false, Message = "Old password is incorrect" };
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await _userRepository.SaveChangesAsync();

        return new AuthResult { Success = true, Message = "Password changed successfully" };
    }

    public async Task<AuthResult> UpdateProfilePictureAsync(int userId, string profilePicturePath)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Success = false, Message = "User not found" };
        }

        user.ProfilePicture = profilePicturePath;

        await _userRepository.SaveChangesAsync();

        return new AuthResult { Success = true, Message = "Profile picture updated successfully" };
    }
}
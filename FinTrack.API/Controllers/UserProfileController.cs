using FinTrack.API.DTOs;
using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserProfileController(IAuthService authService)
    {
        _authService = authService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = GetUserId();
            var profile = await _authService.GetUserProfileAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(profile);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching profile", error = ex.Message });
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var result = await _authService.UpdateProfileAsync(userId, dto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating profile", error = ex.Message });
        }
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var result = await _authService.ChangePasswordAsync(userId, dto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while changing password", error = ex.Message });
        }
    }

    [HttpPost("upload-picture")]
    public async Task<IActionResult> UploadPicture(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Invalid file type. Only JPG, JPEG, PNG, GIF are allowed." });
            }

            // Validate file size (e.g., max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "File size exceeds 5MB." });
            }

            var userId = GetUserId();
            var user = await _authService.GetUserProfileAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Create uploads directory if not exists
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            // Generate unique filename
            var fileName = $"{userId}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsDir, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update user profile picture path
            var updateDto = new UpdateProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };

            // Wait, I need to update the ProfilePicture in the user model.
            // But UpdateProfileAsync doesn't have ProfilePicture.
            // I need to modify UpdateProfileAsync to include ProfilePicture.

            // For now, directly update the user.
            // Since AuthService has access to repository, let's add a method or modify.

            // To keep it simple, let's add ProfilePicture to UpdateProfileDto.

            // Update UpdateProfileDto to include ProfilePicture.
            // But for upload, set it separately.

            // Since it's file upload, perhaps create a separate method.

            // For simplicity, let's modify the user directly here, but since controller shouldn't access repo, let's add a method to AuthService.

            // Add UpdateProfilePictureAsync to IAuthService and AuthService.

            // Let's do that.

            // First, add to IAuthService.

            // I already have the interface open.

            // Let's add Task<AuthResult> UpdateProfilePictureAsync(int userId, string profilePicturePath);

            // Then in AuthService, implement it.

            // Then call it here.

            var profilePicturePath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/uploads/profiles/{fileName}";
            var updateResult = await _authService.UpdateProfilePictureAsync(userId, profilePicturePath);
            if (!updateResult.Success)
            {
                return BadRequest(new { message = updateResult.Message });
            }

            return Ok(new { message = "Profile picture updated successfully", profilePicture = profilePicturePath });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while uploading picture", error = ex.Message });
        }
    }
}
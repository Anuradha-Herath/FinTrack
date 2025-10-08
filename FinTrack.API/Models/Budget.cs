using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Budget
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Category { get; set; } // e.g., Food, Transport, Entertainment
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Limit amount must be greater than 0")]
    public decimal LimitAmount { get; set; } // e.g., 5000
    
    [Required]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
    public int Month { get; set; } // 1-12
    
    [Required]
    [Range(2000, 2100, ErrorMessage = "Year must be valid")]
    public int Year { get; set; }
    
    // Optional: Track when budget was created/modified
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
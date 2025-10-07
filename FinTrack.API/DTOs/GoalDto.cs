using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class GoalDto
{
    public int Id { get; set; }
    [Required]
    public required string Title { get; set; } // e.g., "Buy a Laptop"
    [Required]
    public decimal TargetAmount { get; set; } // e.g., 200000
    public decimal CurrentAmount { get; set; } // e.g., 80000
    [Required]
    public DateTime Deadline { get; set; }
    public string? Description { get; set; }
    public decimal ProgressPercentage { get; set; } // Calculated field
}
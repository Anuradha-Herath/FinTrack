using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class BudgetDto
{
    [Required]
    [MaxLength(50)]
    public required string Category { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Limit amount must be greater than 0")]
    public decimal LimitAmount { get; set; }
    
    [Required]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
    public int Month { get; set; }
    
    [Required]
    [Range(2000, 2100, ErrorMessage = "Year must be valid")]
    public int Year { get; set; }
}

public class BudgetResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal LimitAmount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal SpentAmount { get; set; } // Calculated from transactions
    public decimal RemainingAmount { get; set; } // LimitAmount - SpentAmount
    public double ProgressPercentage { get; set; } // (SpentAmount / LimitAmount) * 100
    public string Status { get; set; } = "on-track"; // on-track, warning, over-budget
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    [Required]
    public required string Type { get; set; } // "Income" or "Expense"
    [Required]
    public required string Category { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public int? AccountId { get; set; }
    public string? AccountName { get; set; }
}
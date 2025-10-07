using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    [Required]
    public required string Type { get; set; } // "Income" or "Expense"
    [Required]
    public required string Category { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
    public int? CategoryId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Transaction
{
    public int Id { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int AccountId { get; set; }
    public Account? Account { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
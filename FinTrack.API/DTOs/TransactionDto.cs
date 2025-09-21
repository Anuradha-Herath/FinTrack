using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CategoryId { get; set; }
}
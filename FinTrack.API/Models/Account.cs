using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Account
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Type { get; set; } // e.g., Checking, Savings
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}
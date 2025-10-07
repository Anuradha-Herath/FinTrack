using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Account>? Accounts { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Budget>? Budgets { get; set; }
    public ICollection<Goal>? Goals { get; set; }
}
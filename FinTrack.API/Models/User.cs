using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class User
{
    public int UserId { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    public string? Phone { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalIncome { get; set; } = 0;
    public decimal TotalExpense { get; set; } = 0;
    public ICollection<Account>? Accounts { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Budget>? Budgets { get; set; }
    public ICollection<Goal>? Goals { get; set; }
}
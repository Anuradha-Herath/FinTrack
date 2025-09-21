using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Goal
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime TargetDate { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
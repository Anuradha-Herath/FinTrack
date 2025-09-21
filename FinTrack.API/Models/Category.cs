using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}
namespace Shop.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class Image : BaseEntity
{
    [Required]
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? AltText { get; set; } 
    
}
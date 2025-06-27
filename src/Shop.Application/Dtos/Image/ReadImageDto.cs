namespace Shop.Application.Dtos.Image;

public class ReadImageDto {
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? AltText { get; set; } 
}

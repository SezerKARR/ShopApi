namespace Shop.Application.Dtos.Address;

using System.ComponentModel.DataAnnotations;

public class CreateAddressDto {
    public int? UserId { get; set; }
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string ContactName { get; set; } 

    [Required(ErrorMessage = "Telefon numarası gereklidir.")]
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = null!;
    [Required(ErrorMessage = "Şehir bilgisi gereklidir.")]
    public int CityId { get; set; }
    [Required(ErrorMessage = "İlçe/Semt bilgisi gereklidir.")]
    public int DistrictId { get; set; }
    
    public int NeighborhoodId { get; set; }
   

    [Required(ErrorMessage = "Açık adres gereklidir.")]
    [MaxLength(500)]
    public string AddressLine1 { get; set; } = null!;

    [MaxLength(250)]
    public string? AddressLine2 { get; set; }
}

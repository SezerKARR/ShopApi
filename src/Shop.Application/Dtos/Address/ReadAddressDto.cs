namespace Shop.Application.Dtos.Address;

using System.ComponentModel.DataAnnotations;
using Domain.Models.AddressEntities;
using User;

public class ReadAddressDto {
    public int? UserId { get; set; }
    public virtual ReadUserDto? User { get; set; }
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
    public virtual City? City { get; set; }
    [Required(ErrorMessage = "İlçe/Semt bilgisi gereklidir.")]
    public int DistrictId { get; set; }
    public virtual District? District { get; set; }
    
    public int NeighborhoodId { get; set; }
    public virtual Neighbourhood? Neighborhood { get; set; } 

    [Required(ErrorMessage = "Açık adres gereklidir.")]
    [MaxLength(500)]
    public string AddressLine1 { get; set; } = null!;

    [MaxLength(250)]
    public string? AddressLine2 { get; set; }

}

namespace Shop.Domain.Models.AddressEntities;

using System.ComponentModel.DataAnnotations.Schema;
using Shop.Domain.Models;

public class District:BaseEntity {
    public int CityId { get; set; }
    public City? City { get; set; }
    [Column(TypeName = "decimal(6,4)")]
    public decimal? Longitude { get; set; }
    [Column(TypeName = "decimal(6,4)")]
    public decimal? Latitude { get; set; }
    public string? Coordinates { get; set; }
    public List<Neighbourhood>? Neighbourhoods { get; set; }
}

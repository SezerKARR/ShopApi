namespace Shop.Domain.Models.AddressEntities;

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Shop.Domain.Models;

public class City:BaseEntity {
    
    public int PlateNumber { get; set; }
    [Column(TypeName = "decimal(6,4)")]
    public decimal? Longitude { get; set; }
    [Column(TypeName = "decimal(6,4)")]
    public decimal? Latitude { get; set; }
    public string? Coordinates { get; set; }
    public List<District>? Districts { get; set; }
}

namespace Shop.Domain.Models.AddressEntities;

public class Neighbourhood:BaseEntity{
    public int DistrictId { get; set; }
    public District? District { get; set; }
    public int? ZipCode { get; set; }
    
}

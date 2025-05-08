namespace Shop.Domain.Models.AddressEntities;


public class dto {
    public string Province { get; set; }
    public int PlateNumber { get; set; }
    public string Coordinates { get; set; }
    public List<DistrictDto> Districts { get; set; }
}

public class DistrictDto {
    public string District { get; set; }
    public string Coordinates { get; set; }
    public List<Towns> Towns { get; set; }
}

public class Towns {
    public string Town { get; set; }
    public int ZipCode { get; set; }
    public List<string> Neighbourhoods { get; set; }
}




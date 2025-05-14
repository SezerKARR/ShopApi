namespace Shop.Application.JsonToSql.Converter;

using System.Globalization;
using System.Transactions;
using Domain.Models.AddressEntities;
using Helpers;
using Infrastructure.Data;
using Newtonsoft.Json;

public class AddressJsonToSql {
    IUnitOfWork _unitOfWork;
    AppDbContext _context;
    public AddressJsonToSql(AppDbContext context, IUnitOfWork unitOfWork) {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    public async Task Adjust() {
        var data =await _unitOfWork.DistrictRepository.GetAllAsync();
        foreach (var district in data)
        {
            string[] parts = district.Coordinates.Split(',');
            Console.WriteLine(parts[0]);

            decimal lat=0,longi=0;
            if (parts.Length == 2 &&
                decimal.TryParse(parts[0].Replace(".",","), out decimal latitude) &&
                decimal.TryParse(parts[1].Replace(".",","), out decimal longitude))
            {
                district.Latitude = latitude;
                district.Longitude = longitude;
                Console.WriteLine($"Latitude: {latitude}");
                Console.WriteLine($"Longitude: {longitude}");
            }
            else
            {
                await _unitOfWork.RollbackAsync();
                return;
                Console.WriteLine("Invalid coordinate format.");
            }
           
        }
        await _context.SaveChangesAsync();
        
    }
    public async Task CreateAddressJsonToSql() {
      

        string solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName; // ShopApi/
        string jsonFilePath = Path.Combine(solutionRoot, "Shop.Application", "JsonToSql", "Json", "turkey-geo.json");
        Console.WriteLine(jsonFilePath);
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        var data = JsonConvert.DeserializeObject<List<dto>>(jsonData);

        await _unitOfWork.BeginTransactionAsync();
        Console.WriteLine(data[1].Province);
        try
        {
            foreach (var city in data)
            {
                string[] parts = city.Coordinates.Split(',');
                Console.WriteLine(parts[0]);

                decimal lat=0,longi=0;
                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0].Replace(".",","), out decimal latitude) &&
                    decimal.TryParse(parts[1].Replace(".",","), out decimal longitude))
                {
                    lat=latitude;
                    longi=longitude;
                    Console.WriteLine($"Latitude: {latitude}");
                    Console.WriteLine($"Longitude: {longitude}");
                }
                else
                {
                    await _unitOfWork.RollbackAsync();
                    return;
                    Console.WriteLine("Invalid coordinate format.");
                }
                City tempCity = new City()
                {
                    Name = city.Province,
                    Slug = SlugHelper.GenerateSlug(city.Province),
                    PlateNumber = city.PlateNumber,
                    Coordinates = city.Coordinates,
                    Latitude = lat,
                    Longitude = longi

                };
                _context.Cities.Add(tempCity);
                await _context.SaveChangesAsync();



                foreach (var district in city.Districts)
                {
                    var latlongdis = district.Coordinates.Split(",");
                    string[] partsdistrict = city.Coordinates.Split(',');
                    decimal latdistrict=0,longidistrict=0;
                    if (parts.Length == 2 &&
                        decimal.TryParse(partsdistrict[0].Replace(".",","), out decimal latitudedistrict) &&
                        decimal.TryParse(partsdistrict[1].Replace(".",","), out decimal longitudedistrict))
                    {
                        latdistrict=latitudedistrict;
                        longidistrict=longitudedistrict;
                        Console.WriteLine($"Latitude: {latitude}");
                        Console.WriteLine($"Longitude: {longitude}");
                    }
                    else
                    {
                        await _unitOfWork.RollbackAsync();
                        return;
                        Console.WriteLine("Invalid coordinate format.");
                    }
                    District tempDistrict = new District()
                    {
                        Name = district.District,
                        Slug = SlugHelper.GenerateSlug(district.District),
                        CityId = tempCity.Id,
                        Latitude = latdistrict,
                        Longitude = longidistrict,
                        Coordinates = district.Coordinates
                    };
                    _context.Districts.Add(tempDistrict);
                    await _context.SaveChangesAsync();
                    foreach (var town in district.Towns)
                    {
                        foreach (var neighbourhood in town.Neighbourhoods)
                        {
                            Neighbourhood tempNeighbourhood = new Neighbourhood()
                            {
                                Name=neighbourhood,
                                Slug = SlugHelper.GenerateSlug(neighbourhood),
                                ZipCode = town.ZipCode,
                                DistrictId = tempDistrict.Id,
                            
                            };
                            _context.Neighbourhoods.Add(tempNeighbourhood);
                        
                        }

                    }
                }
               
            }
            await _context.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
        
        // foreach (var VARIABLE in data.dtos)
        // {
        //     Console.
        // }
    }
}
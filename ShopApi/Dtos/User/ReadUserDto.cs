namespace ShopApi.Dtos.User;

using Brand;
using Models;
using Product;

public class ReadUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
   
}
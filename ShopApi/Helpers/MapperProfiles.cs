namespace SaplingStore.Helpers;

using AutoMapper;
using ShopApi.Dtos.Category;
using ShopApi.Dtos.Product;
using ShopApi.Models;

public class MapperProfiles : Profile{
    public  MapperProfiles () {
       CreateMap<ReadProductDto, Product>();
       CreateMap<Product, ReadProductDto>();
       CreateMap<CreateProductDto, Product>();
       
       
       
       CreateMap<ReadCategoryDto, Category>();
       CreateMap<Category, CreateCategoryDto>();
       CreateMap<CreateCategoryDto, Category>();
       CreateMap<Category, ReadCategoryDto>();
       CreateMap<UpdateCategoryDto, Category>();
    }

}
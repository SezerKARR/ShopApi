namespace ShopApi.Helpers;

using AutoMapper;
using Dtos.Basket;
using Dtos.Brand;
using Dtos.BrandCategory;
using Dtos.Category;
using Dtos.FilterValue;
using Dtos.MainCategory;
using Dtos.Product;
using Models;

public class MapperProfiles : Profile{
    public MapperProfiles() {
        CreateMap<ReadProductDto, Product>();
        CreateMap<Product, ReadProductDto>();

        CreateMap<CreateProductDto, Product>();


        CreateMap<CreateFilterValueDto, FilterValue>();

        CreateMap<ReadCategoryDto, Category>();
        CreateMap<Category, CreateCategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, ReadCategoryDto>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<ReadMainCategory, MainCategory>();
        CreateMap<CreateMainCategory, MainCategory>();
        CreateMap<UpdateMainCategory, MainCategory>();
        CreateMap<MainCategory, ReadMainCategory>();

        CreateMap<ReadBasketDto, Basket>();
        CreateMap<UpdateBasketDto, Basket>();
        CreateMap<CreateBasketDto, Basket>();
        CreateMap<Basket, ReadBasketDto>();

        CreateMap<Brand, ReadBrandDto>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();
        
        CreateMap<CreateBrandCategoryDto, BrandCategory>();
        CreateMap<BrandCategory, ReadBrandCategoryDto>();
    }
}
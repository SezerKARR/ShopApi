namespace ShopApi.Helpers;

using AutoMapper;
using Dtos.Basket;
using Dtos.Brand;
using Dtos.BrandCategory;
using Dtos.Category;
using Dtos.FilterValue;
using Dtos.MainCategory;
using Dtos.Product;
using Dtos.ProductFilterValue;
using Dtos.ProductSeller;
using Dtos.Stock;
using Dtos.User;
using Models;

public class MapperProfiles : Profile{
    public MapperProfiles() {
        CreateMap<ReadProductDto, Product>();
        CreateMap<Product, ReadProductDto>()
            .ForMember(dest => dest.FilterValueIds, opt => opt.MapFrom(src => src.FilterValues.Select(pfv => pfv.Id).ToList()))
            .ForMember(dest => dest.ProductSellerIds, opt => opt.MapFrom(src => src.ProductSellers.Select(ps => ps.Id).ToList()));
        CreateMap<CreateProductDto, Product>();

        CreateMap<CreateFilterValueDto, FilterValue>();

        CreateMap<ReadCategoryDto, Category>();
        CreateMap<Category, CreateCategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, ReadCategoryDto>()
            .ForMember(dest => dest.SubCategoryIds, opt => opt.MapFrom(src => src.SubCategories.Select(c => c.Id).ToList()))
            .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()))
            .ForMember(dest=>dest.CategoryFilterIds,opt=>opt.MapFrom(src=>src.Filters.Select(f=>f.Id).ToList()));
        CreateMap<CreateFilterValueDto, FilterValue>();
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
        CreateMap<Stock, ReadStockDto>();
        
        CreateMap<ProductSeller, ReadProductSellerDto>()
            .ForMember(dest => dest.StockIds, opt => opt.MapFrom(src => src.Stocks != null ? src.Stocks.Select(s => s.Id).ToList() : new List<int>()));
        CreateMap<User, ReadUserDto>()
            .ForMember(dest => dest.ProductSellerIds, opt => opt.MapFrom(src => src.ProductSellers != null ? src.ProductSellers.Select(ps => ps.Id).ToList() : new List<int>()))
            .ForMember(dest => dest.ManagedBrandIds, opt => opt.MapFrom(src => src.ManagedBrands != null ? src.ManagedBrands.Select(b => b.Id).ToList() : new List<int>()))
            .ForMember(dest => dest.CreatedProductIds, opt => opt.MapFrom(src => src.CreatedProducts != null ? src.CreatedProducts.Select(p => p.Id).ToList() : new List<int>())); 
        
        CreateMap<ProductFilterValue, ReadProductFilterValueDto>();
    }
}
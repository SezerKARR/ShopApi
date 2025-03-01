namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Product;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using SaplingStore.Helpers;
using QueryableExtensions=QueryableExtensions;

public class ProductService(IMapper mapper, AppDbContext context) : IProductService {
    IQueryable<Product> GetQueryAbleObject() { return context.Products.AsQueryable(); }
    public  async Task<List<ReadProductDto>> GetProductsAsync()
    {
        var products = await context.Products.ToListAsync();
        var readProductDto=products.Select(product=> mapper.Map<ReadProductDto>(product)).ToList();
        return readProductDto;
    }
    public  async Task<ReadProductDto?> GetProductByIdAsync(int id)
    {
        var product = await GetQueryAbleObject().FirstOrDefaultAsync(e=>e.Id==id);
        var readProductDto = mapper.Map<ReadProductDto>(product);
        return readProductDto;
    }
    public async Task<ReadProductDto?> GetProductBySlugAsync(string slug)
    {
        var product = await GetQueryAbleObject().FirstOrDefaultAsync(e=>e.Slug==slug);
        var readProductDto = mapper.Map<ReadProductDto>(product);
        return readProductDto;
    }
    public  async Task<List<ReadProductDto>> GetProductsAsync(QueryObject queryObject)
    {
        var query = GetQueryAbleObject();
        query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
        query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDecSending);
        var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
        List<Product> entities = await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        List<ReadProductDto> readProductDtos=entities.Select(entity=>mapper.Map<ReadProductDto>(entity)).ToList();
        
        return readProductDtos;
    }
    public  async Task<ReadProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {        
        Console.WriteLine(createProductDto.Name);
        Product product = mapper.Map<Product>(createProductDto);
        product.Slug=SlugHelper.GenerateSlug(product.Name);
        await AdjustEntity(product);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return mapper.Map<ReadProductDto>(createProductDto);
    }
    private async Task AdjustEntity(Product entity)
    {
        
        try
        {
            entity.Category =
                await context.Categories.FirstOrDefaultAsync(c => c.Id == entity.CategoryId);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
    public  async Task<ReadProductDto?> UpdateProductAsync(int id, UpdateProductDto dto) 
    {
        var existing = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null) return null;
        mapper.Map(dto, existing);
        await context.SaveChangesAsync();
        return mapper.Map<ReadProductDto>(existing);
    }
    public  async Task<ReadProductDto?> DeleteProductAsync(int id)
    {
        var model = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (model == null) return null;
        context.Products.Remove(model);
        await context.SaveChangesAsync();
        return mapper.Map<ReadProductDto>(model);
    }
    public virtual async Task<bool> ProductExistAsync(int id)
    { 
        return await context.Products.AnyAsync(e => e.Id == id);
    }
   
}
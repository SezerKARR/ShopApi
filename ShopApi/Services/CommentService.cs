namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Category;
using Dtos.Comment;
using Interface;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using SaplingStore.Helpers;
using Shared.Cache;

public class CommentService(IMapper mapper, ICommentRepository commentRepository, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<CommentService> logger):ICommentService {

    public async Task<Response<List<Comment>>> GetCommentsAsync() {
        try
        {
            var comments = await memoryCache.GetOrCreateAsync(CacheKeys.CategoriesList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return commentRepository.GetAsync();
            });
            return new Response<List<Comment>>(comments ?? new List<Comment>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Comments.");
            return new Response<List<Comment>>($"An error occurred while fetching Comments: {ex.Message}");
        }
    }
    public async Task<Response<Comment>> GetCommentByIdAsync(int id) {
        try
        {
            // Ürün listesini Response ile alıyoruz
            Comment comment = await commentRepository.GetTByIdAsync(id);
            if (comment == null) { return new Response<Comment>($"Comment with id: {id} not found."); }
            return new Response<Comment>(comment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Comment by id.");
            return new Response<Comment>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<Comment>> CreateCommentAsync(CreateCommentDto createCommentDto) {
        try
        {
            Comment comment = mapper.Map<Comment>(createCommentDto); 
            comment.Slug = SlugHelper.GenerateSlug(createCommentDto.Name);
            comment.ImageUrls=FormManager.Save(createCommentDto.Images,"uploads/Comments",FormTypes.Image);
            await commentRepository.CreateAsync(comment);
            if (!await unitOfWork.CommitAsync())
            {
                return new Response<Comment>($"An error occurred when creating product: {comment.Name}.");
            }
            memoryCache.Remove(CacheKeys.CommentsList);
                return new Response<Comment>(comment);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        // try
        // {
        //     Product product = mapper.Map<Product>(createProductDto);
        //     product.Slug = SlugHelper.GenerateSlug(product.Name);
        //     string rootPath=Directory.GetCurrentDirectory()+"/wwwroot";
        //     product.ImageUrl=FormManager.Save(createProductDto.ImageFile, rootPath,"uploads/products",FormTypes.Image);
        //     await AdjustEntity(product);
        //     await productRepository.CreateAsync(product);
        //     if (!await unitOfWork.CommitAsync())
        //     {
        //         return new Response<Product>($"An error occurred when creating product: {product.Name}.");
        //     }
        //     
        //     memoryCache.Remove(CacheKeys.ProductsList);
        //     return new Response<Product>(product);
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError(ex, "Could not save product.");
        //     return new Response<Product>($"An error occurred when saving the product: {ex.Message}");
        // }
    }
}

public interface ICommentService {
}
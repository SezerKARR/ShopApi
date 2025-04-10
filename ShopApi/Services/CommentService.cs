namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Comment;
using Helpers;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface ICommentService {
    Task<Response<List<ReadCommentDto>>> GetCommentsAsync();
    Task<Response<ReadCommentDto>> GetCommentByIdAsync(int id);
    Task<Response<ReadCommentDto>> CreateCommentAsync(CreateCommentDto createCommentDto);
}

public class CommentService : ICommentService {
    readonly IMapper _mapper;
    readonly ICommentRepository _commentRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CommentService> _logger;
    public CommentService(IMapper mapper, ICommentRepository commentRepository, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<CommentService> logger) {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Response<List<ReadCommentDto>>> GetCommentsAsync() {
        try
        {
            List<Comment>? comments = await _memoryCache.GetOrCreateAsync(CacheKeys.CategoriesList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _commentRepository.GetAllAsync();
            });
            List<ReadCommentDto> readCommentDtos = _mapper.Map<List<ReadCommentDto>>(comments);

            return new Response<List<ReadCommentDto>>(readCommentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Comments.");
            return new Response<List<ReadCommentDto>>($"An error occurred while fetching Comments: {ex.Message}");
        }
    }
    public async Task<Response<ReadCommentDto>> GetCommentByIdAsync(int id) {
        try
        {
            Comment? comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) { return new Response<ReadCommentDto>($"Comment with id: {id} not found."); }
            ReadCommentDto readCommentDto = _mapper.Map<ReadCommentDto>(comment);
            return new Response<ReadCommentDto>(readCommentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Comment by id.");
            return new Response<ReadCommentDto>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<ReadCommentDto>> CreateCommentAsync(CreateCommentDto createCommentDto) {
        bool isProductExist = await _unitOfWork.ProductRepository.AnyAsync( createCommentDto.ProductId);
    
        if (!isProductExist)
            return new Response<ReadCommentDto>("Product does not exist");
        try
        {
            Comment comment = _mapper.Map<Comment>(createCommentDto); 
            comment.Slug = SlugHelper.GenerateSlug(createCommentDto.Name);
            if (createCommentDto.Images!=null)
            {
                comment.ImageUrls=FormManager.Save(createCommentDto.Images,"uploads/Comments",FormTypes.Image);

            }
            await _commentRepository.CreateAsync(comment);
            if (!await _unitOfWork.CommitAsync())
            {
                return new Response<ReadCommentDto>($"An error occurred when creating product: {comment.Name}.");
            }
            _memoryCache.Remove(CacheKeys.CommentsList);
            ReadCommentDto readCommentDto = _mapper.Map<ReadCommentDto>(comment);
            return new Response<ReadCommentDto>(readCommentDto);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
}
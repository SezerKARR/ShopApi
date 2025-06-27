namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Cache;
using Shop.Infrastructure.Data;

public class FilterService:IFilterService {
    readonly IMapper _mapper;
    readonly IFilterRepository _filterRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CommentService> _logger;
    public FilterService(IMapper mapper,  IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<CommentService> logger ) {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _filterRepository = unitOfWork.FilterRepository;
    }
    public async Task<Response<Filter>> CreateFilter(Filter filter) {
       
    
        
        try
        {
            
            filter.Slug = SlugHelper.GenerateSlug(filter.Name);
          
            await _filterRepository.CreateAsync(filter);
            if (!await _unitOfWork.CommitAsync())
            {
                return new Response<Filter>($"An error occurred when creating filter: {filter.Name}.");
            }
            _memoryCache.Remove(CacheKeys.CommentsList);
            return new Response<Filter>(filter);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<Filter>($"An error occurred when creating filter: {filter.Name}.");
        }
        
    }
    public async Task<Response<List<Filter>>> GetFilters(int includes=-1) {
        try
        {
            var filters = await _filterRepository.GetAllAsync(includes);
            return new Response<List<Filter>>(filters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<List<Filter>>(e.Message);
        }
    }
    public async Task<Response<Filter>> GetFilterById(int id,int includes=-1) {
        try
        {
            var filter = await _filterRepository.GetByIdAsync(id,includes);
            if (filter == null)
            {
                return new Response<Filter>($"doesnt find any filter by this id. id:{id}");
            }
            return new Response<Filter>(filter);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<Filter>(e.Message);
        }
    }
    public async Task<Response<List<Filter>>> GetFiltersByCategoryId(int categoryId,int includes=-1) {
        try
        {
            var filters = await _filterRepository.GetFiltersByCategoryId(categoryId,includes);
            if (!filters.Any())
            {
                return new Response<List<Filter>>($"No filters found for category with ID: {categoryId}");
            }
            return new Response<List<Filter>>(filters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<List<Filter>>(e.Message);
        }
    }
    
}

public interface IFilterService {
    public  Task<Response<List<Filter>>> GetFiltersByCategoryId(int categoryId,int includes=-1);
    public  Task<Response<Filter>> CreateFilter(Filter filter);
    public  Task<Response<List<Filter>>> GetFilters(int includes=-1);
    public  Task<Response<Filter>> GetFilterById(int id,int includes=-1);
}
namespace ShopApi.Services;

using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Data;
using Dtos.Brand;
using Helpers;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IBrandService {
    Task<Response<List<ReadBrandDto>>> GetBrandsAsync();
    Task<Response<ReadBrandDto?>> GetBrandByIdAsync(int id);
    Task<Response<ReadBrandDto>> GetBrandBySlugAsync(string slug);
    Task<Response<ReadBrandDto>> CreateBrandAsync(CreateBrandDto createDto);
    Task<Response<ReadBrandDto>> UpdateBrandAsync(int id, UpdateBrandDto updateDto);
    Task<Response<ReadBrandDto>> DeleteBrandAsync(int id);
    Task<Response<bool>> BrandExistsAsync(int id);
}

public class BrandService :IBrandService {
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<BrandService> _logger;
    private readonly IBrandRepository _brandRepository;

    public BrandService(IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<BrandService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
        _logger = logger;
        _brandRepository = unitOfWork.BrandRepository;
    }

    public async Task<Response<List<ReadBrandDto>>> GetBrandsAsync()
    {
        try
        {
            var brands = await _memoryCache.GetOrCreateAsync(CacheKeys.BrandList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _brandRepository.GetAllAsync();
            });

            var dto = _mapper.Map<List<ReadBrandDto>>(brands);
            return new Response<List<ReadBrandDto>>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching brands.");
            return new Response<List<ReadBrandDto>>($"Error fetching brands: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandDto?>> GetBrandByIdAsync(int id)
    {
        try
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) return new Response<ReadBrandDto?>("Brand not found");

            var dto = _mapper.Map<ReadBrandDto>(brand);
            return new Response<ReadBrandDto?>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching brand by id.");
            return new Response<ReadBrandDto?>($"Error: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandDto>> GetBrandBySlugAsync(string slug)
    {
        try
        {
            var brand = await _brandRepository.GetBySlugAsync(slug);
            if (brand == null) return new Response<ReadBrandDto>("Brand not found");

            var dto = _mapper.Map<ReadBrandDto>(brand);
            return new Response<ReadBrandDto>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching brand by slug.");
            return new Response<ReadBrandDto>($"Error: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandDto>> CreateBrandAsync(CreateBrandDto createDto)
    {
        try
        {
            var brand = _mapper.Map<Brand>(createDto);
            brand.Slug = SlugHelper.GenerateSlug(brand.Name);

            await _brandRepository.CreateAsync(brand);
            await _unitOfWork.CommitAsync();

            _memoryCache.Remove("brands_list");

            var dto = _mapper.Map<ReadBrandDto>(brand);
            return new Response<ReadBrandDto>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating brand.");
            return new Response<ReadBrandDto>($"Error: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandDto>> UpdateBrandAsync(int id, UpdateBrandDto updateDto)
    {
        try
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) return new Response<ReadBrandDto>("Brand not found");

            _mapper.Map(updateDto, brand);

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                brand.Slug = SlugHelper.GenerateSlug(updateDto.Name);

            _brandRepository.Update(brand);
            await _unitOfWork.CommitAsync();

            _memoryCache.Remove("brands_list");

            var dto = _mapper.Map<ReadBrandDto>(brand);
            return new Response<ReadBrandDto>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating brand.");
            return new Response<ReadBrandDto>($"Error: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandDto>> DeleteBrandAsync(int id)
    {
        try
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) return new Response<ReadBrandDto>("Brand not found");

            _brandRepository.Delete(brand);
            await _unitOfWork.CommitAsync();

            _memoryCache.Remove("brands_list");

            var dto = _mapper.Map<ReadBrandDto>(brand);
            return new Response<ReadBrandDto>(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting brand.");
            return new Response<ReadBrandDto>($"Error: {ex.Message}");
        }
    }

    public async Task<Response<bool>> BrandExistsAsync(int id)
    {
        try
        {
            var exists = await _brandRepository.AnyAsync(id);
            return new Response<bool>(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking brand existence.");
            return new Response<bool>($"Error: {ex.Message}");
        }
    }
}
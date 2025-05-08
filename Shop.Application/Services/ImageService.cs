namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Image;
using Helpers;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

public class ImageService:IImageService {
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Image> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public ImageService(IImageRepository imageRepository, IMapper mapper, ILogger<Image> logger, IUnitOfWork unitOfWork) {
        _imageRepository = imageRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response<ReadImageDto>> CreateImageAsync(CreateImageDto createImageDto, bool useTransaction = true) {
        if (useTransaction)
        {
            await _unitOfWork.BeginTransactionAsync();

        }
        try
        {
            var image = new Image
            {
                CreatedAt = DateTime.Now,IsActive = true,AltText = createImageDto.AltText,Name = createImageDto.AltText
                ,Slug = SlugHelper.GenerateSlug(createImageDto.AltText)
            };
            image.Url = FormManager.Save(createImageDto.ImageFile, "uploads/productImage", FormTypes.Image);
            await _imageRepository.CreateAsync(image);
            if (!await _unitOfWork.CommitAsync())
            {
                await _unitOfWork.RollbackAsync();
                return new Response<ReadImageDto>($"An error occurred when creating product Image: {image.Id}");

            }
            if (useTransaction)
            {
                await _unitOfWork.CommitTransactionAsync();

            }
            ReadImageDto readProductImageDto = _mapper.Map<ReadImageDto>(image);
            return new Response<ReadImageDto>(readProductImageDto);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            return new Response<ReadImageDto>(e.Message);
        }
    }
}

public interface IImageService {
    Task<Response<ReadImageDto>> CreateImageAsync(CreateImageDto createImageDto, bool useTransaction = true);
}
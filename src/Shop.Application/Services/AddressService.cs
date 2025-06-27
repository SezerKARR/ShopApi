namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Address;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

public interface IAddressService {
    Task<Response<ReadAddressDto>> GetAddressByIdAsync(int id);
    Task<Response<ReadAddressDto>> CreateAddress(CreateAddressDto createAddressDto);
}

public class AddressService:IAddressService {
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Address> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public AddressService(IAddressRepository addressRepository, IMapper mapper, ILogger<Address> logger, IUnitOfWork unitOfWork) {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response<ReadAddressDto>> GetAddressByIdAsync(int id) {
       
        try
        {
            var address =await _addressRepository.GetByIdAsync(id);
            if (address == null)
                return new Response<ReadAddressDto>($"Address {id} not found");
            ReadAddressDto readAddressDto = _mapper.Map<ReadAddressDto>(address);
            return new Response<ReadAddressDto>(readAddressDto);
    
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ReadAddressDto>(e.Message);
        }
    
    }
    public async Task<Response<ReadAddressDto>> CreateAddress(CreateAddressDto createAddressDto) {
        var district = await _unitOfWork.DistrictRepository.GetByIdAsync(createAddressDto.DistrictId);
        
        if (district == null)
            return new Response<ReadAddressDto>("District not found");
        Console.WriteLine(district.Name,createAddressDto.DistrictId);
        if (district.CityId != createAddressDto.CityId)
            return new Response<ReadAddressDto>("City id not match");
        var neighBorHood = await _unitOfWork.NeighborhoodRepository.GetByIdAsync(createAddressDto.NeighborhoodId);
        if (neighBorHood == null)
            return new Response<ReadAddressDto>("Neighborhood not found");
        if (neighBorHood.DistrictId != createAddressDto.DistrictId)
            return new Response<ReadAddressDto>("District id not match");
        try
        {
            var address = _mapper.Map<Address>(createAddressDto);
            await _addressRepository.CreateAsync(address);
            if (!await _unitOfWork.CommitAsync())
            {
                _logger.LogError("Failed to create address");
                return new Response<ReadAddressDto>("Failed to create address");
            }
            await _unitOfWork.CommitAsync();
            ReadAddressDto readAddressDto = _mapper.Map<ReadAddressDto>(address);
            return new Response<ReadAddressDto>(readAddressDto);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ReadAddressDto>(e.Message);
        }

    }
}
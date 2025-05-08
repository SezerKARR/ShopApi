namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Address;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

public class AddressService {
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
    // public async Task<Response<Address>> CreateAddress(CreateAddressDto createAddressDto) {
    //     var address = _mapper.Map<Address>(createAddressDto);
    //     
    // }
}

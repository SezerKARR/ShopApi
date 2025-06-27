namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models.Common;
using Dtos.Seller;
using Infrastructure.Repository;
using Shop.Infrastructure.Data;

public interface ISellerService {
    Task<Response<ReadSellerDto>> GetSellerByIdAsync(int id,int includes);
}

public class SellerService:ISellerService {
    private readonly ISellerRepository _sellerRepository;
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public SellerService( ISellerRepository sellerRepository,IUnitOfWork unitOfWork, IMapper mapper) {
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // public async Task<ReadSellerDto> GetOrCreateUserAsync(string email, string name)
    // {
    //     var user = await _userRepository.GetBySlugAsync(SlugHelper.GenerateSlug(name));
    //     if (user == null)
    //     {
    //         user = new User { Email = email, Name = name };
    //         await _userRepository.CreateAsync(user);
    //         await _unitOfWork.CommitAsync();
    //     }
    //     return user;
    // }
    public async Task<Response<ReadSellerDto>> GetSellerByIdAsync(int id,int includes) {
        var seller = await _sellerRepository.GetByIdAsync(id,includes);
        if (seller == null)
            return new Response<ReadSellerDto>("Seller not found");
        ReadSellerDto readSellerDto=_mapper.Map<ReadSellerDto>(seller);
        return new Response<ReadSellerDto>(readSellerDto);
    }
}

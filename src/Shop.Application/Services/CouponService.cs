namespace Shop.Application.Services;

using System.Diagnostics;
using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Coupon;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Shop.Infrastructure.Data;

public interface ICouponService {
    Task<Response<ReadCouponDto>> GetCouponByIdAsync(int id);
    Task<Response<ReadCouponDto>> CreateCouponAsync(CreateCouponDto createCouponDto);
    Task<Response<List<ReadCouponDto>>> GetCouponsAsync();
    Task<Response<List<ReadCouponDto>>> GetCouponsBySellerIdAsync(int sellerId,int includes=-1);
}

public class CouponService:ICouponService {
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CouponService> _logger;
    private readonly IMapper _mapper;
    public CouponService(ICouponRepository couponRepository, IMapper mapper, ILogger<CouponService> logger, IUnitOfWork unitOfWork) {
        _couponRepository = couponRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response<List<ReadCouponDto>>> GetCouponsAsync() {
        var coupons = await _couponRepository.GetAllAsync();
        Debug.Assert(coupons != null, nameof(coupons) + " != null");
        coupons=coupons.Where(c=>c.IsExpired==false).ToList();
        if(!coupons.Any())
            return new Response<List<ReadCouponDto>>("There are no coupons");
        List<ReadCouponDto> couponsDto = _mapper.Map<List<ReadCouponDto>>(coupons);
        return new Response<List<ReadCouponDto>>(couponsDto);
    }
    public async Task<Response<List<ReadCouponDto>>> GetCouponsBySellerIdAsync(int sellerId,int includes=-1) {
        var coupons=await _couponRepository.GetCouponBySellerIdAsync(sellerId,includes);
         var availableCoupons=coupons.Where(c=>c.IsExpired==false).ToList();
         if (!availableCoupons.Any())
         {
             return new Response<List<ReadCouponDto>>("There are no coupons");
         }
         List<ReadCouponDto> couponsDto = _mapper.Map<List<ReadCouponDto>>(availableCoupons);
         return new Response<List<ReadCouponDto>>(couponsDto);
    }
    public async Task<Response<ReadCouponDto>> GetCouponByIdAsync(int id) {
        var coupon =await _couponRepository.GetByIdAsync(id);
        
        //todo: eğer auth admin ise active olmayanlar gönderilmeyecek
        if (coupon == null && coupon?.IsExpired==true )
        {
            return new Response<ReadCouponDto>("Coupon not found");
        }
        var readCouponDto = _mapper.Map<ReadCouponDto>(coupon);
        return new Response<ReadCouponDto>(readCouponDto);
    }
    public async Task<Response<ReadCouponDto>> CreateCouponAsync(CreateCouponDto createCouponDto) {
        var seller=await _unitOfWork.SellerRepository.GetByIdAsync(createCouponDto.SellerId);
        
        if (seller == null)
            return new Response<ReadCouponDto>("Product seller not found");
        try
        {
            var coupon = _mapper.Map<Coupon>(createCouponDto);
            // coupon.ValidUntil=createCouponDto.
            await _couponRepository.CreateAsync(coupon);
            await _unitOfWork.CommitAsync();
            var readCouponDto = _mapper.Map<ReadCouponDto>(coupon);
            return new Response<ReadCouponDto>(readCouponDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ReadCouponDto>(e.Message);
        }
    }
}

namespace Shop.Api.Controllers;

using Application.Dtos.Coupon;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CouponController:ControllerBase {
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService) {
        _couponService = couponService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ReadCouponDto>>> GetCoupons() {
        var response = await _couponService.GetCouponsAsync();
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadCouponDto>> GetCouponByIdAsync(int id) {
        var response = await _couponService.GetCouponByIdAsync(id);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpGet("bySellerId/{sellerId:int}")]
    public async Task<ActionResult<List<ReadCouponDto>>> GetCouponsBySellerId(int sellerId,int includes=-1) {
        try
        {
            var response=await _couponService.GetCouponsBySellerIdAsync(sellerId, includes);
            if (response.Success)
            {
                return Ok(response.Resource);
            }
            return BadRequest(response.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<CreateCouponDto>> CreateCoupon(CreateCouponDto createCoupon) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await _couponService.CreateCouponAsync(createCoupon);
            if (response.Success)
            {
                var coupon = response.Resource;
                if (coupon == null) return BadRequest(response.Message);
                return CreatedAtAction("GetCouponById", new{id=coupon.Id}, coupon);

            }
            else { return BadRequest(response.Message); }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}

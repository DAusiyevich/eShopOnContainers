using Coupon.API.DTOs;
using Coupon.API.Infrastructure;
using Coupon.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coupon.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController: ControllerBase
    {
        private readonly EShopContext _eshopContext;

        public CouponController(EShopContext eshopContext, IIdentityService identityService) =>
            _eshopContext = eshopContext;

        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CouponDto>> GetCouponByCodeAsync(string code)
        {
            var coupon = await _eshopContext.FindByCodeAsync(code);

            if (coupon is null || coupon.Consumed)
            {
                return NotFound();
            }

            return new CouponDto
            {
                Code = code,
                Consumed = coupon.Consumed,
                Discount = coupon.Discount
            };
        }
    }
}

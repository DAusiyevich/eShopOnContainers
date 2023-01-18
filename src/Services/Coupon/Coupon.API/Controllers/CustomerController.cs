using Coupon.API.DTOs;
using Coupon.API.Infrastructure;
using Coupon.API.Infrastructure.Models;
using Coupon.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Coupon.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController: ControllerBase
    {
        private EShopContext _eshopContext;
        private readonly IIdentityService _identityService;

        public CustomerController(EShopContext eshopContext, IIdentityService identityService) =>
            (_eshopContext, _identityService) = (eshopContext, identityService);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomer()
        {
            string userId;
            Customer customer;
            try
            {
                userId = _identityService.GetUserIdentity();
                customer = await _eshopContext.CustomersCollection.Find(x => string.Equals(x.CustomerId, userId)).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (customer is null)
            {
                var lowestTier = await _eshopContext.LoyaltyTiersCollection.Find(x => x.LowerLimit == 100).FirstOrDefaultAsync();

                return new CustomerDto
                {
                    PointsAvaliable = 25,
                    MoneySpent = 0,
                    LoyaltyTier = new LoyaltyTierDto
                    {
                        Description = lowestTier.Description,
                        Discount = lowestTier.Discount,
                        Name = lowestTier.Name
                    }
                };
            }

            return new CustomerDto
            {
                PointsAvaliable = customer.PointsAvaliable,
                MoneySpent = customer.MoneySpent,
                LoyaltyTier = new LoyaltyTierDto
                {
                    Description = customer.Tier.Description,
                    Discount = customer.Tier.Discount,
                    Name = customer.Tier.Name
                }
            };
        }
    }
}

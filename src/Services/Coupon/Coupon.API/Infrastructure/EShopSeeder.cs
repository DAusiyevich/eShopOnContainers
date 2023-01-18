using Coupon.API.Infrastructure.Models;
using Microsoft.Extensions.Azure;
using MongoDB.Driver;

namespace Coupon.API.Infrastructure
{
    public class EShopSeeder
    {
        EShopContext _eshopContext;

        public EShopSeeder(EShopContext eshopContext) =>
            _eshopContext = eshopContext;

        public async Task SeedCouponAsync()
        {
            if (await _eshopContext.CountCouponsAsync() == 0)
            {
                var coupons = new List<Models.Coupon>
                {
                    new Models.Coupon { Code = "DISC-5", Discount = 5 },
                    new Models.Coupon { Code = "DISC-10", Discount = 10 },
                    new Models.Coupon { Code = "DISC-15", Discount = 15 },
                    new Models.Coupon { Code = "DISC-20", Discount = 20 },
                    new Models.Coupon { Code = "DISC-25", Discount = 25 },
                    new Models.Coupon { Code = "DISC-30", Discount = 30 },
                    new Models.Coupon { Code = "DISC-35", Discount = 35 },
                    new Models.Coupon { Code = "DISC-40", Discount = 40 }
                };

                await _eshopContext.InsertManyAsync(coupons);
            }

            if (await _eshopContext.LoyaltyTiersCollection.EstimatedDocumentCountAsync() == 0)
            {
                var tiers = new List<LoyaltyTier>
                {
                    new LoyaltyTier
                    {
                        Name = "No Tier",
                        Description = "Spent more money to achieve Bronze tier",
                        LowerLimit = 0,
                        UpperLimit = 100,
                        Discount = 0
                    },
                    new LoyaltyTier
                    {
                        Name = "Bronze",
                        Description = "Spent more money to achieve Silver tier",
                        LowerLimit = 100,
                        UpperLimit = 500,
                        Discount = 3,
                    },
                    new LoyaltyTier
                    {
                        Name = "Silver",
                        Description = "Spent more money to achieve Gold tier",
                        LowerLimit = 500,
                        UpperLimit = 1000,
                        Discount = 4
                    },
                    new LoyaltyTier
                    {
                        Name = "Gold",
                        Description = "You are great!!!",
                        LowerLimit = 1000,
                        UpperLimit = 99999999,
                        Discount = 5
                    }
                };

                await _eshopContext.LoyaltyTiersCollection.InsertManyAsync(tiers);
            }

            if (await _eshopContext.CustomersCollection.EstimatedDocumentCountAsync() == 0)
            {
                var bronzeTier = await _eshopContext.LoyaltyTiersCollection.Find(x => string.Equals(x.Name, "Bronze")).FirstOrDefaultAsync();
                var customer = new Customer
                {
                    CustomerId = "123-123",
                    MoneySpent = 0,
                    PointsAvaliable = 0,
                    Tier = bronzeTier
                };

                await _eshopContext.CustomersCollection.InsertOneAsync(customer);
            }
        }
    }
}

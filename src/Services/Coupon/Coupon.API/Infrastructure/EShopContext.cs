using Coupon.API.Infrastructure.Models;
using MongoDB.Driver;

namespace Coupon.API.Infrastructure
{
    public class EShopContext
    {
        public IMongoCollection<Models.Coupon> CouponsCollection { get; init; }

        public IMongoCollection<LoyaltyTier> LoyaltyTiersCollection { get; init; }

        public IMongoCollection<Customer> CustomersCollection { get; init; }

        public EShopContext()
        {
            var mongoClient = new MongoClient("mongodb://nosqldata:27017");

            var mongoDatabase = mongoClient.GetDatabase("eshop");

            CouponsCollection = mongoDatabase.GetCollection<Models.Coupon>("coupons");
            LoyaltyTiersCollection = mongoDatabase.GetCollection<LoyaltyTier>("loyaltyTiers");
            CustomersCollection = mongoDatabase.GetCollection<Customer>("customers");
        }

        public async Task<Models.Coupon> FindByCodeAsync(string code) => 
            await CouponsCollection
                    .Find(x => string.Equals(x.Code, code))
                    .FirstOrDefaultAsync();

        public async Task InsertManyAsync(IEnumerable<Models.Coupon> coupons) =>
            await CouponsCollection.InsertManyAsync(coupons);

        public async Task<long> CountCouponsAsync() =>
            await CouponsCollection.EstimatedDocumentCountAsync();
    }
}

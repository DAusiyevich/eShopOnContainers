using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Coupon.API.Infrastructure.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public LoyaltyTier Tier { get; set; }

        public int PointsAvaliable { get; set; }

        public decimal MoneySpent { get; set; }
    }
}

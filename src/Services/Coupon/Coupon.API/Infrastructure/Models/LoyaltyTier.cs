using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Coupon.API.Infrastructure.Models
{
    public class LoyaltyTier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float LowerLimit { get; set; }

        public float UpperLimit { get; set;}

        public int Discount { get; set; }
    }
}

namespace Coupon.API.DTOs
{
    public class CustomerDto
    {
        public int PointsAvaliable { get; set; }

        public LoyaltyTierDto LoyaltyTier { get; set; }

        public decimal MoneySpent { get; set; }
    }
}

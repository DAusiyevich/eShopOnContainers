namespace Ordering.API.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToAwaitingCouponValidationIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; init; }

        public string UserId { get; init; }

        public string CouponCode { get; init; }

        public int PointsUser { get; init; }

        public OrderStatusChangedToAwaitingCouponValidationIntegrationEvent(int orderId, string couponCode, int pointsUsed, string userId)
            => (OrderId, CouponCode, PointsUser, UserId) = (orderId, couponCode, pointsUsed, userId);
    }
}

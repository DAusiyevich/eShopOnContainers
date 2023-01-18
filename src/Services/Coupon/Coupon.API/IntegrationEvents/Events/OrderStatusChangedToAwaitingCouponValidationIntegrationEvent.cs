using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Coupon.API.IntegrationEvents.Events
{
    public record OrderStatusChangedToAwaitingCouponValidationIntegrationEvent: IntegrationEvent
    {
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public string CouponCode { get; init; }

        public int PointsUsed { get; init; }

        public OrderStatusChangedToAwaitingCouponValidationIntegrationEvent(int orderId, string couponCode, int pointsUsed, string userId)
            => (OrderId, CouponCode, PointsUsed, UserId) = (orderId, couponCode, pointsUsed, userId);
    }
}

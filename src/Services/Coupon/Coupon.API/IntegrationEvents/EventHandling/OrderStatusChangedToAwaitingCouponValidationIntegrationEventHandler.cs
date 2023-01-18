using Coupon.API.Infrastructure;
using Coupon.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using MongoDB.Driver;

namespace Coupon.API.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToAwaitingCouponValidationIntegrationEventHandler
        : IIntegrationEventHandler<OrderStatusChangedToAwaitingCouponValidationIntegrationEvent>
    {
        private readonly ILogger<OrderStatusChangedToAwaitingCouponValidationIntegrationEventHandler> _logger;
        private readonly EShopContext _eshopContext;
        private readonly IEventBus _eventBus;

        public OrderStatusChangedToAwaitingCouponValidationIntegrationEventHandler(
            ILogger<OrderStatusChangedToAwaitingCouponValidationIntegrationEventHandler> logger,
            EShopContext eshopContext,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _eshopContext = eshopContext ?? throw new System.ArgumentNullException(nameof(eshopContext));
            _eventBus = eventBus ?? throw new System.ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OrderStatusChangedToAwaitingCouponValidationIntegrationEvent @event)
        {
            var isNoCoupon = string.IsNullOrEmpty(@event.CouponCode);
            var isNoPointsUsed = @event.PointsUsed == 0;
            var coupon = await _eshopContext.CouponsCollection
                                                .Find(x => string.Equals(x.Code, @event.CouponCode) && !x.Consumed)
                                                .FirstOrDefaultAsync();

            var customer = await _eshopContext.CustomersCollection
                                                            .Find(x => string.Equals(x.CustomerId, @event.UserId))
                                                            .FirstOrDefaultAsync();

            if ((isNoCoupon || coupon != null) && (isNoPointsUsed || customer.PointsAvaliable >= @event.PointsUsed))
            {
                if (!isNoCoupon)
                {
                    coupon.Consumed = true;
                    coupon.OrderId = @event.OrderId;
                    await _eshopContext.CouponsCollection.ReplaceOneAsync(x => x.Id == coupon.Id, coupon);
                }

                if (!isNoPointsUsed)
                {
                    customer.PointsAvaliable -= @event.PointsUsed;
                    await _eshopContext.CustomersCollection.ReplaceOneAsync(x => x.Id == customer.Id, customer);
                }

                var orderCouponConfirmedIntegrationEvent = new OrderCouponConfirmedIntegrationEvent(@event.OrderId);
                _eventBus.Publish(orderCouponConfirmedIntegrationEvent);
            }
            else
            {
                var orderCouponRejectedIntegrationEvent = new OrderCouponRejectedIntegrationEvent(@event.OrderId);
                _eventBus.Publish(orderCouponRejectedIntegrationEvent);
            }
        }
    }
}

namespace Ordering.API.Application.IntegrationEvents.Events
{
    public record OrderHasBeenPaidIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public int BuyerId { get; }

        public decimal Total { get; }

        public OrderHasBeenPaidIntegrationEvent(int orderId, int buyerId, decimal total) =>
            (OrderId, BuyerId, Total) = (orderId, buyerId, total);
    }
}

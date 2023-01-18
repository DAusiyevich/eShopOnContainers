using Ordering.API.Application.IntegrationEvents.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.API.Application.Commands
{
    public class AssignPointsCommandHandler : IRequestHandler<AssignPointsCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

        public AssignPointsCommandHandler(IOrderRepository orderRepository, IOrderingIntegrationEventService orderingIntegrationEventService) =>
            (_orderRepository, _orderingIntegrationEventService) = (orderRepository, orderingIntegrationEventService);

        public async Task<bool> Handle(AssignPointsCommand command, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(command.OrderNumber);
            if (order == null)
            {
                return false;
            }

            //todo calculate total accroding to discounts
            var @event = new OrderHasBeenPaidIntegrationEvent(order.Id, order.GetBuyerId.Value, 100);
            await _orderingIntegrationEventService.AddAndSaveEventAsync(@event);
            return true;
        }
    }
}

namespace Ordering.API.Application.Commands
{
    public class AssignPointsCommand : IRequest<bool>
    {
        [DataMember]
        public int OrderNumber { get; set; }

        public AssignPointsCommand(int orderId) => 
            OrderNumber = orderId;
    }
}

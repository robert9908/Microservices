using MassTransit;
using SharedMessages;

namespace SagaOrchestration
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }

        public State InventoryReserved { get; private set; }

        public State PaymentCompleted { get; private set; }


        public Event<OrderPlaced> OrderPlacedEvent { get; private set; }
        public Event <InventoryReserved> InventoryReservedEvent { get; private set; }
        public Event<PaymentCompleted> PaymentCompletedEvent { get; private set;}

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => InventoryReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));

            Initially(
                When(OrderPlacedEvent)
                .Then(context =>
                {
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.Quantity = context.Data.Quantity;
                    Console.WriteLine($" Order {context.Instance.OrderId} placed succesfully");
                }).TransitionTo(Submitted));
            During(Submitted,
                When(InventoryReservedEvent)
                .TransitionTo(InventoryReserved));
            During(InventoryReserved,
                When(PaymentCompletedEvent)
                .Then(context => Console.WriteLine($"Order {context.Instance.OrderId} completed"))
                .TransitionTo(PaymentCompleted)
                .Finalize());

            SetCompletedWhenFinalized();
        }
    }
}

using EventSourcing;
using EventSourcingGrains;

namespace Account.Grains.Reconciler
{
    public class AccountReconcilerAggregate : IAggregate<AccountReconciler, IAccountReconcilerEvent>
    {
        public AccountReconciler State { get; set; }
        public void Apply(IAccountReconcilerEvent @event)
        {
            switch (@event)
            {
                default:
                    State.lastProcessedEventId = @event.EventId;
                    break;
            }
        }
    }
}
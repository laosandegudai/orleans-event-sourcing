using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Logging;
using EventSourcingGrains.Stream;
using EventSourcing.Persistance;
using Orleans.Placement;

namespace EventSourcingGrains.Grains
{
    [Reentrant]
    [PreferLocalPlacement]
    public class AggregateStreamDispatcherGrain : EventSourceGrain<AggregateStreamState, IStreamEvent>, IAggregateStreamDispatcherGrain
    {
        private Queue<AggregateEvent> _eventQueue = new Queue<AggregateEvent>();
        private EventDispatcherSettings _eventDispatcherSettings;
        private readonly ILogger<AggregateStreamDispatcherGrain> _logger;
        private bool _isNotifyingSubscribers = false;
        private long _lastQueuedEventId;
        private long _lastNotifiedEventId;
        private string _aggregateName;
        private static TimeSpan _pollingInterval = TimeSpan.FromSeconds(1);
        public const string AggregateName = "aggregate_stream_dispatcher";

        public AggregateStreamDispatcherGrain(ILogger<AggregateStreamDispatcherGrain> logger): base(AggregateName, new AggregateStream())
        {
            _logger = logger;
        }
        
        public override async Task OnActivateAsync()
        {
            // set current grain key as aggregateName
            var nameSplit = GrainReference.GetPrimaryKeyString().Split(':');
            var rootAggregateName = nameSplit[0];
            _aggregateName = nameSplit[1];

            // get IAggregateStreamSettings instance from service collection filtered by grainKey
            var aggregateStreamSettings = ServiceProvider.GetServices<IAggregateStreamSettings>().FirstOrDefault(g => g.AggregateName == rootAggregateName);

            // throw if not able to get StreamSettings
            if(aggregateStreamSettings == null)
            {
                throw new InvalidOperationException($"unable to retrieve IAggregateStreamSettings for {nameSplit[0]}");
            }

            // store grain resolver
            if (!aggregateStreamSettings.EventDispatcherSettingsMap.TryGetValue(_aggregateName, out _eventDispatcherSettings))
            {
                throw new InvalidOperationException($"unable to retrieve EventDispatcherSettings for {_aggregateName}");
            }

            if (_eventDispatcherSettings.PersistDispatcherState)
            {
                // call base OnActivateAsync
                await base.OnActivateAsync();

                // set LastNotifiedEventVersion if needed
                if (State.LastNotifiedEventId == 0)
                {
                    // get last event from db
                    var lastEvent = await EventSource.GetLastAggregateEvent(rootAggregateName);
                    
                    // get LastNotifiedEventVersion as latest aggregate version from db or 0
                    await ApplyEvent(new UpdatedLastNotifiedEventId{ LastNotifiedEventId = lastEvent?.Id ?? 0 });

                    // sync up lastQueuedEventId and LastNotifiedEventId
                    _lastQueuedEventId = State.LastNotifiedEventId;
                }

                // sync up lastQueuedEventId and LastNotifiedEventId
                _lastQueuedEventId = State.LastNotifiedEventId;

            }

            // register pollForEvents method
            this.RegisterTimer(NotifySubscribers, null, TimeSpan.FromSeconds(1), _pollingInterval);
        }

        public Task AddToQueue(Immutable<AggregateEvent[]> events)
        {
            // add new events to queue
            foreach (var ev in events.Value)
            {
                // only add to queue if new(just in case)
                if (ev.Id > _lastQueuedEventId)
                {
                    _eventQueue.Enqueue(ev);
                    _lastQueuedEventId = ev.Id;
                }
            }

            return Task.CompletedTask;
        }

        private async Task NotifySubscribers(object args)
        {
            // if already notifying then return
            if (_isNotifyingSubscribers)
            {
                return;
            }
            // set flag to true
            _isNotifyingSubscribers = true;
            try
            {
                // dequeue each event and sent to subscribers
                while(_eventQueue.Count > 0)
                {
                    var @event = _eventQueue.Peek();
                    // TODO: set up auto retry
                    try
                    {
                        // get subscriber grain via resolver (resolver can return null if there is no need to notify)
                        var subscriberGrain = _eventDispatcherSettings.ReceiverGrainResolver.Invoke(@event, GrainFactory);
                        if (subscriberGrain != null)
                        {
                            // send event
                            await subscriberGrain.Receive(@event);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, $"Error NotifySubscriber {_aggregateName}");
                    }

                    if (_eventDispatcherSettings.PersistDispatcherState)
                    {
                        // update state with LastNotifiedEventVersion
                        await ApplyEvent(new UpdatedLastNotifiedEventId{ LastNotifiedEventId = @event.Id });
                    }
                    else
                    {
                        _lastNotifiedEventId = @event.Id;
                    }

                    // remove from queue
                    _eventQueue.Dequeue();
                }
            }
            finally
            {
                // set flag to false
                _isNotifyingSubscribers = false;
            }
        }

        public ValueTask<long> GetLastQueuedEventId()
        {
            return new ValueTask<long>(State.LastNotifiedEventId);
        }
    }
}
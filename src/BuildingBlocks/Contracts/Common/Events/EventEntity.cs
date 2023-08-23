using Contracts.Common.Interfaces;
using Contracts.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Common.Events
{
    public class EventEntity<TKey> : EntityBase<TKey>, IEventEntity<TKey>
    {
        private readonly List<BaseEvent> _domainEvents = new();
        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
           _domainEvents.Clear();
        }

        public IReadOnlyCollection<BaseEvent> GetDomainEvents() 
            => _domainEvents.AsReadOnly();

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);

        }
    }
}

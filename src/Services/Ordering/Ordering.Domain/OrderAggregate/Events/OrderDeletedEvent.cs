using Contracts.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.OrderAggregate.Events
{
    public class OrderDeletedEvent : BaseEvent
    {
        public long Id { get; private set; }
       

        public OrderDeletedEvent(long id)
        {
            Id = id;
        }
    }
}

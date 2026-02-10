using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Notifications.IntegrationEvents
{
    public sealed record EventCreatedIntegrationEvent(
        Guid Id,
        DateTime OccurredOn,
        Guid EventId,
        string Title,
        DateTime ScheduledAt
    );
}

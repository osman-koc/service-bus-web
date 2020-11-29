using Microsoft.Azure.ServiceBus.Management;
using ServiceBusWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusWeb.Infrastructures
{
    public interface IAzureServiceBus
    {
        Task<QueueDetailViewModel> GetQueueByName(string queueName, bool isLookDeadletter = false);
        Task<IEnumerable<QueueDescription>> GetQueues();
    }
}

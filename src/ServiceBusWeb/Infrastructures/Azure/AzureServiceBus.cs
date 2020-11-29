using Microsoft.Azure.ServiceBus.Management;
using ServiceBusWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusWeb.Infrastructures
{
    public class AzureServiceBus : IAzureServiceBus
    {
        private readonly string _connectionString;
        public AzureServiceBus(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<QueueDetailViewModel> GetQueueByName(string queueName, bool isLookDeadletter = false)
        {
            if (string.IsNullOrEmpty(queueName)) return null;

            var managementClient = new ManagementClient(_connectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(queueName);
            if (queue == null) return null;

            var model = new QueueDetailViewModel
            {
                Path = queue.Path,
                MessageCount = queue.MessageCount,
                CreatedAt = queue.CreatedAt,
                UpdatedAt = queue.UpdatedAt,
                AccessedAt = queue.AccessedAt,

                ActiveMessageCount = queue.MessageCountDetails.ActiveMessageCount,
                DeadLetterMessageCount = queue.MessageCountDetails.DeadLetterMessageCount,
                ScheduledMessageCount = queue.MessageCountDetails.ScheduledMessageCount,
                TransferMessageCount = queue.MessageCountDetails.TransferMessageCount,
                TransferDeadLetterMessageCount = queue.MessageCountDetails.TransferDeadLetterMessageCount,
            };

            await managementClient.CloseAsync();
            return model;
        }

        public async Task<IEnumerable<QueueDescription>> GetQueues()
        {
            var managementClient = new ManagementClient(_connectionString);
            IList<QueueDescription> queues = await managementClient.GetQueuesAsync();
            await managementClient.CloseAsync();
            return queues;
        }
    }
}

using Microsoft.Azure.ServiceBus.Management;
using System;

namespace ServiceBusWeb.Models
{
    public class QueueDetailViewModel
    {
        public string Path { get; set; }
        public long MessageCount { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime AccessedAt { get; set; }
        public DeadletterMessageBody DeadletterMessageBody { get; set; }

        #region Detail
        public long ActiveMessageCount { get; set; }
        public long DeadLetterMessageCount { get; set; }
        public long ScheduledMessageCount { get; set; }
        public long TransferMessageCount { get; set; }
        public long TransferDeadLetterMessageCount { get; set; }
        #endregion
    }
}

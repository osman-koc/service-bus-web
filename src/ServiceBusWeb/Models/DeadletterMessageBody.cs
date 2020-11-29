namespace ServiceBusWeb.Models
{
    public class DeadletterMessageBody
    {
        public string RequestID { get; set; }
        public string WorkplaceID { get; set; }
    }
}

using System.Collections.Generic;

namespace ServiceBusWeb.Models
{
    public class QueueListViewModel
    {
        public QueueListViewModel()
        {
            QueueList = new List<QueueDetailViewModel>();
        }
        public bool RefreshPage { get; set; } = true;
        public List<QueueDetailViewModel> QueueList { get; set; }
    }
}

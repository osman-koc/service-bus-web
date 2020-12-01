using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using ServiceBusWeb.Infrastructures;
using ServiceBusWeb.Models;

namespace ServiceBusWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureServiceBus _serviceBus;

        public HomeController(ILogger<HomeController> logger, IAzureServiceBus serviceBus)
        {
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public async Task<IActionResult> Index()
        {
            var queueList = await GetQueueList();
            var model = new QueueListViewModel { QueueList = queueList };
            return View(model);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return Redirect("/Error");

        }

        [HttpGet]
        public async Task<IActionResult> GetQueueDetailByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                _logger.LogWarning("Queue path is null.");
                return Json(new { state = false, message = "Queue path is null.", data = "" });
            }
            var model = await GetQueueDetail(path, true);
            if (model == null)
            {
                _logger.LogWarning("Queue not found.");
                return Json(new { state = false, message = "Queue not found.", data = "" });
            }
            return PartialView("_QueueDetailPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> RefreshQueueList()
        {
            var model = await GetQueueList();
            return PartialView("_QueueListPartial", model);
        }


        #region PrivateMethods

        private async Task<List<QueueDetailViewModel>> GetQueueList()
        {
            var queueList = new List<QueueDetailViewModel>();
            var result = await _serviceBus.GetQueues();
            foreach (QueueDescription item in result)
            {
                var que = await _serviceBus.GetQueueByName(item.Path);
                if (que != null)
                {
                    que.Status = item.Status;
                    queueList.Add(que);
                }
            }
            return queueList.OrderByDescending(x => x.ActiveMessageCount).OrderByDescending(x => x.DeadLetterMessageCount).ToList();
        }

        private async Task<QueueDetailViewModel> GetQueueDetail(string path, bool isLookDeadletter = false)
        {
            _logger.LogDebug($"GetQueueDetail ({path})");
            var pathDetail = await _serviceBus.GetQueueByName(path, isLookDeadletter);
            return pathDetail;
        }

        #endregion
    }
}

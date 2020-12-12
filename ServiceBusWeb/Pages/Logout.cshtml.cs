using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ServiceBusWeb.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        public LogoutModel(ILogger<LogoutModel> logger)
        {
            this._logger = logger;
        }
        public void OnGet()
        {
            _logger.LogDebug("Logout - Deleting all cookies..");
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            LocalRedirect("/Login");
        }
    }
}

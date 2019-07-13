using Microsoft.AspNetCore.Mvc;

namespace smartacfe.Controllers
{
    public class AdminPortalController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApiUsage()
        {
            return View();
        }
    }
}
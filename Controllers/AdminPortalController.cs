using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smartacfe.Controllers
{
    [Authorize]
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
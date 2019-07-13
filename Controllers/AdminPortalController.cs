using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartacfe.Models;
using smartacfe.Services;

namespace smartacfe.Controllers
{
    [Authorize]
    public class AdminPortalController : Controller
    {
        private readonly ACDeviceService _deviceService;
 
        public AdminPortalController(ACDeviceService deviceService)
        {
            _deviceService = deviceService;
        }
        
        // GET
        public async Task<IActionResult> Index(String filter)
        {
            var devices = await _deviceService.ListDevices(filter);
            var viewModel = new DashboardViewModel()
            {
                Devices = devices,
                Search = filter
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Device(int id)
        {
            var device = await _deviceService.GetDeviceByID(id);
            if (device == null)
            {
                return NotFound();
            }

            var readings = device.ACDeviceReading.AsQueryable().OrderByDescending(r => r.ReadingDateTime);

            var viewModel = new DeviceDetailsViewModel()
            {
                Device = device,
                LatestReading = readings.FirstOrDefault()
            };
            return View(viewModel);
        }

        [Route("/AdminPortal/Device/{id}/Stats")]
        public async Task<IActionResult> TemperatureStats(int id)
        {
            return Json(new
            {
                Today = await _deviceService.GetHourlyAveragesForDay(id, DateTime.Today),
                ThisWeek = await _deviceService.GetDailyAveragesForRange(id, DateTime.Today.AddDays(-6),
                    DateTime.Today),
                ThisMonth = await _deviceService.GetDailyAveragesForRange(id, DateTime.Today.AddMonths(-1),
                    DateTime.Today),
                ThisYear = await _deviceService.GetMonthlyAveragesForRange(id, DateTime.Today.AddYears(-1),
                    DateTime.Today)
            });
        }

        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _deviceService.GetUnClearedAlerts();
            return Json(notifications);
        }

        public async Task<IActionResult> ClearNotification(int id)
        {
            await _deviceService.ClearAlert(id);
            return Json(new {result = "success"});
        }

        public async Task<IActionResult> ClearAllNotifications()
        {
            await _deviceService.ClearAllAlerts();
            return Json(new {result = "success"});
        }
    }
}
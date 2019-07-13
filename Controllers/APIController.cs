using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using smartacfe.Data;
using smartacfe.Models;
using smartacfe.Services;

namespace smartacfe.Controllers
{
    
    public class APIController : Controller
    {
        private readonly APIService _apiService;
       
        public APIController(APIService service)
        {
            _apiService = service;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("SerialNumber,FirmwareVersion")] [FromBody] ACDevice device)
        {
            var newDevice = await _apiService.RegisterDevice(device.SerialNumber, device.FirmwareVersion);
            return Json(new
            {
                response = "success",
                device = new
                {
                    SerialNumber = newDevice.SerialNumber,
                    FirmwareVersion = newDevice.FirmwareVersion,
                    RegistrationDate = newDevice.RegistrationDate,
                    AccessKey = newDevice.AccessKey
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Reading([FromBody] IEnumerable<ACDeviceReading> readings)
        {
            if (!this.Request.Headers.ContainsKey("x-access-key"))
            {
                return new ForbidResult();
            }
            var accessKey = this.Request.Headers["x-access-key"];
            var device = await _apiService.GetACDeviceByAccessKey(accessKey);

            if (device == null)
            {
                return new NotFoundResult();
            }
            
            readings.ToList().ForEach(i =>
            {
                i.ACDevice = device;
                i.LoggedDateTime = DateTime.Now;
            });
            await _apiService.RecordReadings(readings);
            return Json(new {response = "success"});
        }
    }
}
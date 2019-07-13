using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using smartacfe.Data;
using smartacfe.Models;
using System.Collections.Generic;
using System.Linq;

namespace smartacfe.Services
{
    public class APIService
    {
        private DBContext _context;
        public APIService(DBContext context)
        {
            _context = context;
        }

        public async Task<ACDevice> RegisterDevice(string SerialNumber, string FirmwareVersion)
        {
            var device = await _context.ACDevices.SingleOrDefaultAsync(d => d.SerialNumber == SerialNumber);

            if (device != null)
            {
                throw new Exception("Device already registered");
            }

            var accessKey = Guid.NewGuid();
            var registered = await _context.ACDevices.AddAsync(new ACDevice()
            {
                SerialNumber = SerialNumber,
                FirmwareVersion = FirmwareVersion,
                RegistrationDate = DateTime.Now,
                AccessKey = accessKey.ToString()
            });
            await _context.SaveChangesAsync();
            return registered.Entity;
        }


        public async Task RecordReadings(IEnumerable<ACDeviceReading> readings)
        {
            // await _context.Database.BeginTransactionAsync();
            await _context.AddRangeAsync(readings);
            await _context.SaveChangesAsync();

            await CheckForAlerts(readings);
        }

        public async Task<ACDevice> GetACDeviceByAccessKey(string AccessKey)
        {
            return await _context.ACDevices.SingleOrDefaultAsync(d => d.AccessKey == AccessKey);
        }

        public async Task CheckForAlerts(IEnumerable<ACDeviceReading> readings)
        {
            var healthAlerts = new string[] {"needs_service", "needs_new_filter", "gas_leak"};
            var coLevelThreshold = 9;

            var alerts = new List<ACDeviceAlert>();
            
            foreach (var reading in readings)
            {
                if (healthAlerts.Contains(reading.HealthStatus))
                {
                    alerts.Add(new ACDeviceAlert()
                    {
                        ACDeviceID = reading.ACDeviceID,
                        AlertDateTime = DateTime.Now,
                        ReadingDateTime = reading.ReadingDateTime,
                        AlertType = DeviceAlertType.HEALTH_STATUS,
                        Cleared = false
                    });
                }

                if (reading.COLevel > coLevelThreshold)
                {
                    alerts.Add(new ACDeviceAlert()
                    {
                        ACDeviceID = reading.ACDeviceID,
                        AlertDateTime = DateTime.Now,
                        ReadingDateTime = reading.ReadingDateTime,
                        AlertType = DeviceAlertType.CO_LEVEL,
                        Cleared = false
                    });
                }
            }

            await _context.AddRangeAsync(alerts); 
            await _context.SaveChangesAsync();

        }
    }
}
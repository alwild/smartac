using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using smartacfe.Data;
using smartacfe.Models;
using System.Collections.Generic;

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
                //TODO:: raise an exception
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


        public void RecordReadings(IEnumerable<ACDeviceReading> readings)
        {
            // I've got something setup wrong somewhere.  When I hit the api with a lot of requests, I get 
            // System.InvalidOperationException: Invalid operation. The connection is closed.
            // in the logs
            _context.Database.BeginTransaction();
            _context.AddRange(readings);
            _context.SaveChanges();
        }

        public async Task<ACDevice> GetACDeviceByAccessKey(string AccessKey)
        {
            return await _context.ACDevices.SingleOrDefaultAsync(d => d.AccessKey == AccessKey);
        }
    }
}
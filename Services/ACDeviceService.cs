using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using smartacfe.Data;
using smartacfe.Models;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace smartacfe.Services
{
    public class ACDeviceService
    {
        private DBContext _context;
        public ACDeviceService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ACDevice>> ListDevices(string SerialNumber)
        {
            var deviceQuery = _context.ACDevices.AsQueryable();
            if (!String.IsNullOrEmpty(SerialNumber))
            {
                deviceQuery = deviceQuery.Where(d => d.SerialNumber.StartsWith(SerialNumber));
            }

            return await deviceQuery.ToListAsync();
        }

        public async Task<ACDevice> GetDeviceByID(int ID)
        {
            return await _context.ACDevices
                .Include(r => r.ACDeviceReading)
                .SingleOrDefaultAsync(d => d.ID == ID);
        }

        public async Task<IEnumerable<ACDeviceAlert>> GetUnClearedAlerts()
        {
            return await _context.ACDeviceAlerts.Where(a => a.Cleared == false)
                .Include(d => d.Device)
                .ToListAsync();
        }

        public async Task ClearAllAlerts()
        {
            await _context.Database.ExecuteSqlCommandAsync("UPDATE ACDeviceAlerts SET Cleared=1");
        }

        public async Task ClearAlert(int ID)
        {
            var alert = await _context.ACDeviceAlerts.SingleOrDefaultAsync(a => a.ID == ID);
            if (alert != null)
            {
                alert.Cleared = true;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<IEnumerable<DateTimeDataPoint>> GetHourlyAveragesForDay(int ID, DateTime date)
        {
            var start = date.Date;
            var end = date.AddDays(1).Date;
            var data = await HourlyGrouping(ID, start, end);
            return data.Select(item => new DateTimeDataPoint()
                {
                    x = new DateTime(date.Year, date.Month, date.Day, item.Key, 0, 0),
                    temperature = item.Average(r => r.Temperature),
                    humidity = item.Average(r => r.Humidity),
                    colevel = item.Average(r => r.COLevel)
                })
                .OrderBy(i => i.x);
        }
        
        public async Task<IEnumerable<DateTimeDataPoint>> GetDailyAveragesForRange(int ID, DateTime start, DateTime end)
        {
            var endInclusive = end.Date.AddDays(1).Date;
            var data = await DailyGrouping(ID, start, endInclusive);
            return data.Select(item => new DateTimeDataPoint()
                {
                    x = item.Key,
                    temperature = item.Average(r => r.Temperature),
                    humidity = item.Average(r => r.Humidity),
                    colevel = item.Average(r => r.COLevel)
                })
                .OrderBy(i => i.x);
        }
        
        public async Task<IEnumerable<DateTimeDataPoint>> GetMonthlyAveragesForRange(int ID, DateTime start, DateTime end)
        {
            var endInclusive = end.Date.AddDays(1).Date;
            var data = await MonthlyGrouping(ID, start, endInclusive);
            return data.Select(item => new DateTimeDataPoint()
                {
                    x = new DateTime(item.Key.Year, item.Key.Month, 1),
                    temperature = item.Average(r => r.Temperature),
                    humidity = item.Average(r => r.Humidity),
                    colevel = item.Average(r => r.COLevel)
                })
                .OrderBy(i => i.x);
        }
        
        private async Task<IEnumerable<IGrouping<dynamic, ACDeviceReading>>> MonthlyGrouping(int ID, DateTime start, DateTime endInclusive)
        {
            return await _context.AcDeviceReadings.Where(r =>
                    r.ACDeviceID == ID && r.ReadingDateTime >= start && r.ReadingDateTime < endInclusive)
                .GroupBy(r => new {Year = r.ReadingDateTime.Year, Month = r.ReadingDateTime.Month})
                .ToListAsync();
        }
        
        private async Task<IEnumerable<IGrouping<DateTime, ACDeviceReading>>> DailyGrouping(int ID, DateTime start, DateTime endInclusive)
        {
            return await _context.AcDeviceReadings.Where(r =>
                    r.ACDeviceID == ID && r.ReadingDateTime >= start && r.ReadingDateTime < endInclusive)
                .GroupBy(r => r.ReadingDateTime.Date)
                .ToListAsync();
        }
        
        private async Task<IEnumerable<IGrouping<int, ACDeviceReading>>> HourlyGrouping(int ID, DateTime start, DateTime endInclusive)
        {
            return await _context.AcDeviceReadings.Where(r =>
                    r.ACDeviceID == ID && r.ReadingDateTime >= start && r.ReadingDateTime < endInclusive)
                .GroupBy(r => r.ReadingDateTime.Hour)
                .ToListAsync();
        }
    }
}
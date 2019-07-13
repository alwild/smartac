using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartacfe.Models
{
    public class ACDeviceReading
    {
        public Int32 ID { get; set; }
        public Int32 ACDeviceID { get; set; }
        public ACDevice ACDevice { get; set; }
        public Decimal Temperature { get; set; }
        public Decimal COLevel { get; set; }
        public Decimal Humidity { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public DateTime LoggedDateTime { get; set; }
        public String HealthStatus { get; set; } 
    }
}
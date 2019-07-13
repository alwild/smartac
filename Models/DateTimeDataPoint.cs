using System;

namespace smartacfe.Models
{
    public class DateTimeDataPoint
    {
        public DateTime x { get; set; }
        public Decimal temperature { get; set; }
        public Decimal humidity { get; set; }
        public Decimal colevel { get; set; }
    }
}
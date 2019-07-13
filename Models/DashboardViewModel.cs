using System;
using System.Collections.Generic;

namespace smartacfe.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<ACDevice> Devices { get; set; }
        public String Search { get; set; }
    }
}
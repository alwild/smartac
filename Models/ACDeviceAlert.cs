using System;

namespace smartacfe.Models
{
    public enum DeviceAlertType
    {
        HEALTH_STATUS,
        CO_LEVEL
    }
    
    public class ACDeviceAlert
    {
        public Int32 ID { get; set; }
        public Int32 ACDeviceID { get; set; }
        public ACDevice Device { get; set; }
        public DeviceAlertType AlertType { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public DateTime AlertDateTime { get; set; }
        public Boolean Cleared { get; set; }
    }
}
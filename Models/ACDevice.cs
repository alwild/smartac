using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartacfe.Models
{
    public class ACDevice
    {
        public Int32 ID { get; set; }
        [Required]
        public String SerialNumber { get; set; }
        [Required]
        public String FirmwareVersion { get; set; }
        public DateTime RegistrationDate { get; set; }
        public String AccessKey { get; set; }
       
        [ForeignKey("ACDeviceID")]
        public ICollection<ACDeviceReading> ACDeviceReading { get; set; }
    }
}
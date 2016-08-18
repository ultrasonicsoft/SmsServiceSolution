using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mitto.SMSService.Models
{
    public class Country
    {
        public string mcc { get; set; }
        public string cc { get; set; }
        public string name { get; set; }
        public double pricePerSMS { get; set; }
    }
}

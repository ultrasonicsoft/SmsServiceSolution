using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mitto.SMSService.Models
{
    public class Sms
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string CountryCode { get; set; }
        public bool IsDelivered { get; set; }
    }
}

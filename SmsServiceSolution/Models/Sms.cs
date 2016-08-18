using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mitto.SMSService.Models;

namespace SmsServiceSolution.Models
{
    public class Sms
    {
        public DateTime dateTime { get; set; }
        public string mcc { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public double price { get; set; }
        public State state { get; set; }
    }
}
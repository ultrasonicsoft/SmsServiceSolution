using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsServiceSolution.Models
{
    public class SmsStatisticsRecord
    {
        public DateTime day { get; set; }
        public string mcc { get; set; }
        public double pricePerSMS { get; set; }
        public int count { get; set; }
        public double totalPrice { get; set; }
    }
}
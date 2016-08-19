using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Mitto.SMSService.Models
{
    public class SmsStatisticsParams
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string MccList { get; set; }
    }
}
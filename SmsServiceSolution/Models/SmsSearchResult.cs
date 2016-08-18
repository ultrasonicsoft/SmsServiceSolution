using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmsServiceSolution.Models;

namespace Mitto.SMSService.Models
{
    public class SmsSearchResult
    {
        public int totalCount { get; set; }
        public List<Sms> items { get; set; }
    }
}
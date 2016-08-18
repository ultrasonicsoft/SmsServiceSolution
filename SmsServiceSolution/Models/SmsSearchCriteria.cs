using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsServiceSolution.Models
{
    public class SmsSearchCriteria
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
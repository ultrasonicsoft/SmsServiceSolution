using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsServiceSolution.SmsDbManager
{
    public class SmsDbQueries
    {
        public const string GetAllCountries = "SELECT * FROM mitto_sms_db.countries;";
        public const string StoreSms = "StoreSms";
        public const string GetSentSms = "GetSentSms";
        public const string GetSmsStatistics = "GetSmsStatistics";
        public const string Login = "Login";
        

    }
}
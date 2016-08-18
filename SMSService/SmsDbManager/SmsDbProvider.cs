using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.DbProvider
{
    public class SmsDbProvider  : ISmsDbProvider
    {
        private string connectionString ;

        public SmsDbProvider(string connectionString)
        {
//            this.connectionString = connectionString;
            this.connectionString = "server=127.0.0.1;uid=root; pwd=12345;database=mitto_sms_db;";
        }

        public bool StoreSms(Sms newSms)
        {
        }
    }
}

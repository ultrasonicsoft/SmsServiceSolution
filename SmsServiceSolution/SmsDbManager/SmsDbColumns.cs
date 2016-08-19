using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsServiceSolution.SmsDbManager
{
    public class SmsDbColumns
    {
        public const string Name = "name";
        public const string Mcc = "mcc";
        public const string CC = "cc";
        public const string Price_per_sms = "price_per_sms";

        public const string _From = "_from";
        public const string _To = "_to";
        public const string Country_code = "_country_code";
        public const string Message_text = "_message_text";
        public const string _IsDelivered = "_isDelivered";

        public const string DateTimeFrom = "dateTimeFrom";
        public const string DateTimeTo = "dateTimeTo";
        public const string Skip = "skip";
        public const string Take = "take";

        public const string Sent_on = "sent_on";
        public const string From = "from";
        public const string To = "to";

        public const string IsDelivered = "is_delivered";

        public const string DateFrom = "dateFrom";
        public const string DateTo = "dateTo";
        public const string MccList = "mccList";

        public const string Day = "day";

        public const string Count = "count";
        public const string TotalPrice = "totalPrice";


        public const string _UserName = "_userName";
        public const string _Password = "_password";
        public const string IsLoginSuccessful = "isLoginSuccessful";
        



    }
}
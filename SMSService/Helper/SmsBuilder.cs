using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public class SmsBuilder : ISmsBuilder
    {
        private readonly int CountryCodePosition = 3;

        public Sms ComposeSms(string from, string to, string text)
        {
            var countryCode = ExtractCountryCode(to);
            var newSms = new Sms
            {
                From = from,
                To = to,
                Text = text,
                CountryCode = countryCode,
                IsDelivered = false
            };
            return newSms;
        }

        private string ExtractCountryCode(string receiverNumber)
        {
            var countryCode = string.Empty;
            try
            {
                countryCode = receiverNumber.Substring(0, CountryCodePosition);
            }
            catch (Exception exception)
            {
                throw;
            }
            return countryCode;
        }
    }
}

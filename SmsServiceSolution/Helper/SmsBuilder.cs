using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public class SmsBuilder : ISmsBuilder
    {
        private readonly int CountryCodePosition = 2;
        private readonly int ToNumberLength = 11;

        public SmsParams ComposeSms(string from, string to, string text)
        {
            var countryCode = ExtractCountryCode(to);
            var toNumber = ExtractToNumber(to);
            var newSms = new SmsParams
            {
                From = from,
                To = toNumber,
                Text = text,
                CountryCode = countryCode,
                IsDelivered = false
            };
            return newSms;
        }

        private string ExtractToNumber(string receiverNumber)
        {
            var toNumber = string.Empty;
            try
            {
                toNumber = receiverNumber.Substring(3, ToNumberLength);
            }
            catch (Exception exception)
            {
                throw;
            }
            return toNumber;
        }

        private string ExtractCountryCode(string receiverNumber)
        {
            var countryCode = string.Empty;
            try
            {
                countryCode = receiverNumber.Substring(1, CountryCodePosition);
            }
            catch (Exception exception)
            {
                throw;
            }
            return countryCode;
        }
    }
}

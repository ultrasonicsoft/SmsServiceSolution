using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public class CountryProvider : ICountryProvider
    {
        public IList<Country> GetAllCountries()
        {
            var allCountries = new List<Country>();

            try
            {
                allCountries = new List<Country>
                {
                    new Country
                    {
                        mcc = "262",
                        cc = "49",
                        name = "Germany",
                        pricePerSMS = 0.055
                    },
                    new Country
                    {
                        mcc = "232",
                        cc = "43",
                        name = "Austria",
                        pricePerSMS = 0.053
                    },
                    new Country
                    {
                        mcc = "260",
                        cc = "48",
                        name = "Poland",
                        pricePerSMS = 0.032
                    }
                };
            }
            catch (Exception exception)
            {
                throw;
            }

            return allCountries;
        }
    }
}

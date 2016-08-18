using System.Collections.Generic;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public interface ICountryProvider
    {
        IList<Country> GetAllCountries();
    }
}
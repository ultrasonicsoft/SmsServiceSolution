using System.Collections.Generic;
using Mitto.SMSService.Models;
using SmsServiceSolution.Models;

namespace Mitto.SMSService.DbProvider
{
    public interface ISmsDbProvider
    {
        IList<Country> GetAllCountries();
        bool StoreSms(SmsParams newSmsParams);
        SmsSearchResult GetSentSms(SmsSearchCriteria smsSearchCriteria);
        IList<SmsStatisticsRecord> GetSmsStatistics(SmsStatisticsParams smsStatisticsParams);
    }
}
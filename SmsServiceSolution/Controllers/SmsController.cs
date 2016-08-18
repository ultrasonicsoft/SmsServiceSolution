using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Mitto.SMSService.DbProvider;
using Mitto.SMSService.Models;
using Mitto.SMSService.Providers;
using SmsServiceSolution.Models;

namespace SmsServiceSolution.Controllers
{
    [RoutePrefix("mitto/sms")]
    public class SmsController : ApiController
    {
        // GET: api/SmsParams
        public IEnumerable<string> Get()
        {
            return new string[] { "Jai", "Ganesh" };
        }

        private readonly ISmsProvider smsProvider = new MockSmsProvider();
        private readonly ISmsBuilder smsBuilder = new SmsBuilder();
        private readonly ISmsDbProvider smsDbProvider = new SmsDbProvider(ConfigurationManager.ConnectionStrings["mitto_sms_db"].ConnectionString);

        public SmsController()
        {
        }

        [Route("countries")]
        [HttpGet]
        public IEnumerable<Country> GetCountries()
        {
            return smsDbProvider.GetAllCountries();
        }

        [Route("sent")]
        [HttpGet]
        public SmsSearchResult GetSentSms([FromUri] string dateTimeFrom, [FromUri]string dateTimeTo, [FromUri]int skip, [FromUri]int take)
        {
            //            var from = DateTime.Parse(dateTimeFrom);
            //            var to = DateTime.Parse(dateTimeTo);
            var smsSearchCriteria = new SmsSearchCriteria
            {
                DateTimeFrom = DateTime.Parse(dateTimeFrom),
                DateTimeTo = DateTime.Parse(dateTimeTo),
                Skip = skip,
                Take = take
            };

            var searchResult = smsDbProvider.GetSentSms(smsSearchCriteria);

            return searchResult;
        }

        [Route("statistics")]
        [HttpGet]
        public IList<SmsStatisticsRecord> GetStatistics([FromUri] string dateFrom, [FromUri]string dateTo, [FromUri]string mccList)
        {

            var combinedMccList = string.Empty;
            if (string.IsNullOrEmpty(mccList))
            {
                combinedMccList = BuildAllMobileCountryCodeList();
            }
            else
            {
                combinedMccList = BuildSelectedMobileCountryCodeList(mccList);
            }

            var smsStatisticsParams = new SmsStatisticsParams
            {
                DateFrom = DateTime.Parse(dateFrom),
                DateTo = DateTime.Parse(dateTo),
                MccList = combinedMccList
            };

            var searchResult = smsDbProvider.GetSmsStatistics(smsStatisticsParams);

            return searchResult;
        }

        private string BuildSelectedMobileCountryCodeList(string mccList)
        {
            var combinedMccList = string.Empty;
            var mccListParts = mccList.Split(',').ToList();
            var mccBuilder = new StringBuilder();
            for (var index = 0; index < mccListParts.Count; index++)
            {
                mccBuilder.Append(string.Format("'{0}'", mccListParts[index]));
                if (index < mccListParts.Count - 1)
                {
                    mccBuilder.Append(",");
                }
                combinedMccList = mccBuilder.ToString();
            }
            return combinedMccList;
        }

        private string BuildAllMobileCountryCodeList()
        {
            var allCountries = smsDbProvider.GetAllCountries();
            var allMobileCountryCodeListBuilder = new StringBuilder();
            var allMobileCountryCodeList = string.Empty;

            for (int index = 0; index < allCountries.Count; index++)
            {
                allMobileCountryCodeListBuilder.Append(string.Format("'{0}'", allCountries[index].mcc));
                if (index < allCountries.Count - 1)
                {
                    allMobileCountryCodeListBuilder.Append(",");
                }
            }
            allMobileCountryCodeList = allMobileCountryCodeListBuilder.ToString();
            return allMobileCountryCodeList;
        }


        [Route("send")]
        [HttpGet]
        public State SendSMS([FromUri] string from, [FromUri]string to, [FromUri]string text)
        {
            var smsDetails = string.Format("From: {0}, To: {1}, Message Text: {2}", from, to, text);

            var newSms = smsBuilder.ComposeSms(from, to, text);

            smsDbProvider.StoreSms(newSms);

            var status = smsProvider.SendSms(newSms);

            return State.Success;
        }
    }
}

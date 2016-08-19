using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Mitto.SMSService.DbProvider;
using Mitto.SMSService.Models;
using Mitto.SMSService.Providers;
using SmsServiceSolution.Models;

namespace SmsServiceSolution.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    [RoutePrefix("mitto/sms")]
    public class SmsController : ApiController
    {
        private readonly ISmsProvider smsProvider = new MockSmsProvider();
        private readonly ISmsBuilder smsBuilder = new SmsBuilder();
        private readonly ISmsDbProvider smsDbProvider = new SmsDbProvider(ConfigurationManager.ConnectionStrings["mitto_sms_db"].ConnectionString);

        public SmsController()
        {
        }

        [HttpGet]
        public List<string> Get()
        {
            return new List<string> {"value1", "value2"};
        }

        [Route("login")]
        [HttpPost]
        public AuthenticationResult DoLogin([FromBody] User user)
        {
            var authenticationResult = new AuthenticationResult();
            try
            {
                var isValidUser = smsDbProvider.DoLogin(user.userName, user.password);
                if (!isValidUser) return authenticationResult;

                authenticationResult.isAuthenticatedUser = true;
                authenticationResult.loggedInUser = user;
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            return authenticationResult;
        }

        [Route("countries")]
        [HttpGet]
        public IEnumerable<Country> GetCountries()
        {
            return smsDbProvider.GetAllCountries();
        }

        [Route("send")]
        [HttpGet]
        public State SendSMS([FromUri] string from, [FromUri]string to, [FromUri]string text)
        {
            State status;
            try
            {
                var newSms = smsBuilder.ComposeSms(from, to, text);
                smsDbProvider.StoreSms(newSms);
                status = smsProvider.SendSms(newSms);
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            return status;
        }

        [Route("sent")]
        [HttpGet]
        public SmsSearchResult GetSentSms([FromUri] string dateTimeFrom, [FromUri]string dateTimeTo, [FromUri]int skip, [FromUri]int take)
        {
            var searchResult = new SmsSearchResult();

            try
            {
                var smsSearchCriteria = new SmsSearchCriteria
                {
                    DateTimeFrom = DateTime.Parse(dateTimeFrom),
                    DateTimeTo = DateTime.Parse(dateTimeTo),
                    Skip = skip,
                    Take = take
                };
                searchResult = smsDbProvider.GetSentSms(smsSearchCriteria);
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            
            return searchResult;
        }

        [Route("statistics")]
        [HttpGet]
        public IList<SmsStatisticsRecord> GetStatistics([FromUri] string dateFrom, [FromUri]string dateTo, [FromUri]string mccList)
        {
            IList<SmsStatisticsRecord> searchResult = new List<SmsStatisticsRecord>();
            try
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

                searchResult = smsDbProvider.GetSmsStatistics(smsStatisticsParams);
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            return searchResult;
        }

        private string BuildSelectedMobileCountryCodeList(string mccList)
        {
            var combinedMccList = string.Empty;
            try
            {
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
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            return combinedMccList;
        }

        private string BuildAllMobileCountryCodeList()
        {
            var allMobileCountryCodeList = string.Empty;
            try
            {
                var allCountries = smsDbProvider.GetAllCountries();
                var allMobileCountryCodeListBuilder = new StringBuilder();

                for (int index = 0; index < allCountries.Count; index++)
                {
                    allMobileCountryCodeListBuilder.Append(string.Format("'{0}'", allCountries[index].mcc));
                    if (index < allCountries.Count - 1)
                    {
                        allMobileCountryCodeListBuilder.Append(",");
                    }
                }
                allMobileCountryCodeList = allMobileCountryCodeListBuilder.ToString();
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            return allMobileCountryCodeList;
        }
    }
}

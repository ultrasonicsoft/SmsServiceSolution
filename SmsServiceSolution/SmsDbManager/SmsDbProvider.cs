using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mitto.SMSService.Models;
using MySql.Data.MySqlClient;
using SmsServiceSolution.Models;

namespace Mitto.SMSService.DbProvider
{
    public class SmsDbProvider : ISmsDbProvider
    {
        private string connectionString;

        public SmsDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Country> GetAllCountries()
        {
            var allCountries = new List<Country>();
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM mitto_sms_db.countries;", connection);
                var adapter = new MySqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                allCountries.AddRange(from DataRow countryRow in dataSet.Tables[0].Rows
                                      select new Country
                                      {
                                          name = countryRow["name"].ToString(),
                                          mcc = countryRow["mcc"].ToString(),
                                          cc = countryRow["cc"].ToString(),
                                          pricePerSMS = double.Parse(countryRow["price_per_sms"].ToString())
                                      });
                connection.Close();
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return allCountries;
        }

        public bool StoreSms(SmsParams newSmsParams)
        {
            var executionStatus = false;
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = new MySqlCommand("StoreSms", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_from", newSmsParams.From);
                command.Parameters.AddWithValue("_to", newSmsParams.To);
                command.Parameters.AddWithValue("_country_code", newSmsParams.CountryCode);
                command.Parameters.AddWithValue("_message_text", newSmsParams.Text);
                command.Parameters.AddWithValue("_isDelivered", false);
                executionStatus = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return executionStatus;
        }

        public SmsSearchResult GetSentSms(SmsSearchCriteria smsSearchCriteria)
        {
            var searchResult = new SmsSearchResult();

            var executionStatus = false;
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = new MySqlCommand("GetSentSms", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("dateTimeFrom", smsSearchCriteria.DateTimeFrom);
                command.Parameters.AddWithValue("dateTimeTo", smsSearchCriteria.DateTimeTo);
                command.Parameters.AddWithValue("skip", smsSearchCriteria.Skip);
                command.Parameters.AddWithValue("take", smsSearchCriteria.Take);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                searchResult.totalCount = dataSet.Tables[0].Rows.Count;

                searchResult.items = new List<Sms>();
                foreach (DataRow searchResultRow in dataSet.Tables[0].Rows)
                {
                    searchResult.items.Add(new Sms
                    {
                        dateTime = DateTime.Parse(searchResultRow["sent_on"].ToString()),
                        mcc = searchResultRow["cc"].ToString(),
                        from = searchResultRow["from"].ToString(),
                        to = searchResultRow["to"].ToString(),
                        price = double.Parse(searchResultRow["price_per_sms"].ToString()),
                        state = (State)Enum.Parse(typeof(State), searchResultRow["is_delivered"].ToString())

                    });
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return searchResult;
        }

        public IList<SmsStatisticsRecord> GetSmsStatistics(SmsStatisticsParams smsStatisticsParams)
        {
            var statisticsResult = new List<SmsStatisticsRecord>();
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                var command = new MySqlCommand("GetSmsStatistics", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("dateFrom", smsStatisticsParams.DateFrom);
                command.Parameters.AddWithValue("dateTo", smsStatisticsParams.DateTo);
                command.Parameters.AddWithValue("mccList", smsStatisticsParams.MccList);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                foreach (DataRow statisticRow in dataSet.Tables[0].Rows)
                {
                    statisticsResult.Add(new SmsStatisticsRecord
                    {
                        day = DateTime.Parse(statisticRow["day"].ToString()),
                        mcc = statisticRow["mcc"].ToString(),
                        pricePerSMS = double.Parse(statisticRow["price_per_sms"].ToString()),
                        count = int.Parse(statisticRow["count"].ToString()),
                        totalPrice = double.Parse(statisticRow["totalPrice"].ToString())
                    });
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return statisticsResult;
        }
    }
}

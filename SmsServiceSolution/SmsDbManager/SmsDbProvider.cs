using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mitto.SMSService.Models;
using MySql.Data.MySqlClient;
using SmsServiceSolution.Models;
using SmsServiceSolution.SmsDbManager;

namespace Mitto.SMSService.DbProvider
{
    public class SmsDbProvider : ISmsDbProvider
    {
        private readonly string connectionString;

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
                var command = new MySqlCommand(SmsDbQueries.GetAllCountries, connection);
                var adapter = new MySqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                allCountries.AddRange(from DataRow countryRow in dataSet.Tables[0].Rows
                                      select new Country
                                      {
                                          name = countryRow[SmsDbColumns.Name].ToString(),
                                          mcc = countryRow[SmsDbColumns.Mcc].ToString(),
                                          cc = countryRow[SmsDbColumns.CC].ToString(),
                                          pricePerSMS = double.Parse(countryRow[SmsDbColumns.Price_per_sms].ToString())
                                      });
                connection.Close();
            }
            catch (Exception exception)
            {
                //TODO: log exception
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
                var command = new MySqlCommand(SmsDbQueries.StoreSms, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(SmsDbColumns._From, newSmsParams.From);
                command.Parameters.AddWithValue(SmsDbColumns._To, newSmsParams.To);
                command.Parameters.AddWithValue(SmsDbColumns.Country_code, newSmsParams.CountryCode);
                command.Parameters.AddWithValue(SmsDbColumns.Message_text, newSmsParams.Text);
                command.Parameters.AddWithValue(SmsDbColumns._IsDelivered, false);
                executionStatus = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
            catch (Exception exception)
            {
                //TODO: log exception
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
                var command = new MySqlCommand(SmsDbQueries.GetSentSms, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(SmsDbColumns.DateTimeFrom, smsSearchCriteria.DateTimeFrom);
                command.Parameters.AddWithValue(SmsDbColumns.DateTimeTo, smsSearchCriteria.DateTimeTo);
                command.Parameters.AddWithValue(SmsDbColumns.Skip, smsSearchCriteria.Skip);
                command.Parameters.AddWithValue(SmsDbColumns.Take, smsSearchCriteria.Take);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                searchResult.totalCount = dataSet.Tables[0].Rows.Count;

                searchResult.items = new List<Sms>();
                foreach (DataRow searchResultRow in dataSet.Tables[0].Rows)
                {
                    searchResult.items.Add(new Sms
                    {
                        dateTime = DateTime.Parse(searchResultRow[SmsDbColumns.Sent_on].ToString()),
                        mcc = searchResultRow[SmsDbColumns.CC].ToString(),
                        from = searchResultRow[SmsDbColumns.From].ToString(),
                        to = searchResultRow[SmsDbColumns.To].ToString(),
                        price = double.Parse(searchResultRow[SmsDbColumns.Price_per_sms].ToString()),
                        state = (State)Enum.Parse(typeof(State), searchResultRow[SmsDbColumns.IsDelivered].ToString())
                    });
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                //TODO: log exception
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

                var command = new MySqlCommand(SmsDbQueries.GetSmsStatistics, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(SmsDbColumns.DateFrom, smsStatisticsParams.DateFrom);
                command.Parameters.AddWithValue(SmsDbColumns.DateTo, smsStatisticsParams.DateTo);
                command.Parameters.AddWithValue(SmsDbColumns.MccList, smsStatisticsParams.MccList);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                foreach (DataRow statisticRow in dataSet.Tables[0].Rows)
                {
                    statisticsResult.Add(new SmsStatisticsRecord
                    {
                        day = DateTime.Parse(statisticRow[SmsDbColumns.Day].ToString()),
                        mcc = statisticRow[SmsDbColumns.Mcc].ToString(),
                        pricePerSMS = double.Parse(statisticRow[SmsDbColumns.Price_per_sms].ToString()),
                        count = int.Parse(statisticRow[SmsDbColumns.Count].ToString()),
                        totalPrice = double.Parse(statisticRow[SmsDbColumns.TotalPrice].ToString())
                    });
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                //TODO: log exception
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

        public bool DoLogin(string userName, string password)
        {
            var isLoginSuccessful = false;

            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = new MySqlCommand(SmsDbQueries.Login, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(SmsDbColumns._UserName, userName);
                command.Parameters.AddWithValue(SmsDbColumns._Password, password);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                var resultCount = int.Parse(dataSet.Tables[0].Rows[0][SmsDbColumns.IsLoginSuccessful].ToString());
                isLoginSuccessful = resultCount > 0;

                connection.Close();
            }
            catch (Exception exception)
            {
                //TODO: log exception
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return isLoginSuccessful;
        }
    }
}

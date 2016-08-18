using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public class MockSmsProvider : ISmsProvider
    {
        private string logFilePath;

        public MockSmsProvider()
        {
            string logFileName = "smsLogs.csv";
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(Path.Combine(appDataPath, "Mitto")))
            {
                Directory.CreateDirectory(Path.Combine(appDataPath, "Mitto"));
            }
            logFilePath = Path.Combine(appDataPath, "Mitto", logFileName);
        }

        public State SendSms(SmsParams newSmsParams)
        {
            State status = State.Success;
            string smsDetails =
                $"From: {newSmsParams.From}, To: {newSmsParams.To}, Message Text: {newSmsParams.Text}, Sent On: {DateTime.Now.ToString(CultureInfo.InvariantCulture)} {Environment.NewLine}";
            Console.WriteLine(smsDetails);
            WriteSmsToLogFile(smsDetails);
            return status;
        }

        private void WriteSmsToLogFile(string smsDetails)
        {
            try
            {
                File.AppendAllText(logFilePath, smsDetails);
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}

using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public interface ISmsBuilder
    {
        SmsParams ComposeSms(string from, string to, string text);
    }
}
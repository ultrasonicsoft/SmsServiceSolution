using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public interface ISmsBuilder
    {
        Sms ComposeSms(string from, string to, string text);
    }
}
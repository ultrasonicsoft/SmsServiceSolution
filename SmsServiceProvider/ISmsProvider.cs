using Mitto.SMSService.Models;

namespace Mitto.SMSService.Providers
{
    public interface ISmsProvider
    {
        State SendSms(SmsParams newSmsParams);
    }
}
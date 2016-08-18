using Mitto.SMSService.Models;

namespace Mitto.SMSService.DbProvider
{
    public interface ISmsDbProvider
    {
        bool StoreSms(Sms newSms);
    }
}
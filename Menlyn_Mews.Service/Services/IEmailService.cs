using Menlyn_Mews.Service.Models;
using Menlyn_Mews.Service.Services;

namespace Menlyn_Mews.Service.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}

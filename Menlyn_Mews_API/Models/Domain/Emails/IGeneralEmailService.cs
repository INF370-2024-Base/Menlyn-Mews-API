using Microsoft.Extensions.Options;

namespace Menlyn_Mews_API.Models.Domain.Emails
{
    public interface IGeneralEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}

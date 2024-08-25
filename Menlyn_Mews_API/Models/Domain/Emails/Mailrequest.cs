using System.Net.Mail;

namespace Menlyn_Mews_API.Models.Domain.Emails
{
    public class Mailrequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}

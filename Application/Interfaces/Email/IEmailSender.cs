using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(string email, string subject, string message,IEnumerable<string> attachmentUrls);
        Task SendEmailAsync(string email, string subject, string message, IEnumerable<Attachment> attachment);
        Task SendEmailAsync(string email,string emailCC, string subject, string message);
        Task SendEmailAsync(IEnumerable<string> emails, string subject, string message);

        Task SendEmailWithTemplateAsysnc(string templateCode, string email, IReadOnlyDictionary<string, string> headerParam, IReadOnlyDictionary<string, string> bodyParam);
    }
}

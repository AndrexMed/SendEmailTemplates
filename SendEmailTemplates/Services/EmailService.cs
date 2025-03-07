using System.Net;
using System.Net.Mail;

namespace SendEmailTemplates.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailEmisor = _configuration.GetValue<string>("Config_Emails:Email");
            var password = _configuration.GetValue<string>("Config_Emails:Password");
            var host = _configuration.GetValue<string>("Config_Emails:Host");
            var port = _configuration.GetValue<int>("Config_Emails:Port");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(emailEmisor, password);
            var message = new MailMessage(emailEmisor!, to, subject, body);

            await smtpClient.SendMailAsync(message);
        }
    }
}

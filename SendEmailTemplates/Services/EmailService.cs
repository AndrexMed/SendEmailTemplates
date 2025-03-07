using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendEmailTemplates.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, IFormFile htmlTemplate);
    }

    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task SendEmailAsync(string to, string subject, IFormFile htmlTemplate)
        {
            var emailEmisor = _configuration.GetValue<string>("Config_Emails:Email");
            var password = _configuration.GetValue<string>("Config_Emails:Password");
            var host = _configuration.GetValue<string>("Config_Emails:Host");
            var port = _configuration.GetValue<int>("Config_Emails:Port");

            if (htmlTemplate == null || htmlTemplate.Length == 0)
                throw new ArgumentException("El archivo de plantilla no puede estar vacío.");

            // Leer el archivo HTML en memoria sin modificarlo
            string body;
            using (var reader = new StreamReader(htmlTemplate.OpenReadStream(), Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            // Configurar cliente SMTP
            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailEmisor, password)
            };

            // Crear y enviar el correo
            using var message = new MailMessage
            {
                From = new MailAddress(emailEmisor!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // <-- Importante para que se renderice el HTML
            };
            message.To.Add(to);

            await smtpClient.SendMailAsync(message);
        }
    }
}

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Covalence {
    
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender 
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Execute(Options.MailGunSmtpUrl, Options.MailGunUser, Options.MailGunPassword, subject, message, email);
        }

        public async Task Execute(string smtpUrl, string smtpUsername, string smtpPassword, string subject, string message, string email)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Covalence", "no-reply@becovalent.com"));
            mail.To.Add(new MailboxAddress(email));
            mail.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = message;
            builder.HtmlBody = message;

            mail.Body = builder.ToMessageBody();

            using(var client = new SmtpClient()) {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(smtpUrl, 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
        }
    }
}
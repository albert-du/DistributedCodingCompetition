namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.Web.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
public class EmailService(IOptions<SMTPOptions> options, ILogger<EmailService> logger) : IEmailService
{
    private readonly SMTPOptions emailConfig = options.Value;
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Distributed Coding Competition", emailConfig.From));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlMessage };

        using SmtpClient client = new();
        await client.ConnectAsync(emailConfig.Host, emailConfig.Port, emailConfig.EnableTLS ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
        await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        logger.LogDebug("Email sent to {address} with subject {subject}", email, subject);
    }
}

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
namespace Websocket_fødselsdag_2.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        this.emailSettings = options.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var fromAddress = new MailAddress(emailSettings.Email);
        var toAddress = new MailAddress(mailRequest.ToEmail);
        string fromPassword = emailSettings.Password;
        string subject = mailRequest.Subject;
        string body = mailRequest.Body;
        
        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        
        using (var message = new MailMessage(fromAddress, toAddress)
               {
                   Subject = subject,
                   Body = body
               })
        {
            smtp.Send(message);
        }
        
        
    }
}
namespace Websocket_fødselsdag_2.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
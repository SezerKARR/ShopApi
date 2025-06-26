namespace Shop.Domain.Models.Messaging.Smtp;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
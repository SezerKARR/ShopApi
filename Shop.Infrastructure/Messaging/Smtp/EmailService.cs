namespace Shop.Infrastructure.Messaging.Smtp;

using System.Net;
using System.Net.Mail;
using Domain.Models.Messaging.Smtp;
using Microsoft.Extensions.Options;

// Shop.Infrastructure/Messaging/Smtp/EmailService.cs

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // 1. SmtpClient'ı oluştur
        var client = new SmtpClient(_settings.Server, _settings.Port)
        {
            // 2. KİMLİK BİLGİLERİNİ AYARLA (En Önemli Kısım)
            // appsettings.json'daki Username ve Password'ü kullanarak bir kimlik oluşturur.
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),

            // 3. GÜVENLİ BAĞLANTIYI ZORUNLU KIL (Diğer Önemli Kısım)
            // Bu, bağlantının şifrelenmesini (SSL/TLS) sağlar.
            EnableSsl = true 
        };

        // ... (MailMessage oluşturma kodu aynı)

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        // Hata muhtemelen bu satırda oluşuyor. Yukarıdaki ayarlar bu sorunu çözecektir.
        await client.SendMailAsync(mailMessage);
    }
}

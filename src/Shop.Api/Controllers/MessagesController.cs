namespace Shop.Api.Controllers;

using System.Text.Json;
using Domain.Models.Messaging;
using Domain.Models.Messaging.Smtp;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessagePublisher _publisher;

    public MessagesController(IMessagePublisher publisher, IMessagePublisher messagePublisher) {
        _publisher = publisher;
        _messagePublisher = messagePublisher;
    }

    [HttpPost]
    public IActionResult Send([FromBody] string message)
    {
        _publisher.Publish("my-queue", message);
        return Ok("Message sent");
    }
    private readonly IMessagePublisher _messagePublisher;

   

    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] string email)
    {

        var emailMessage = new EmailMessage()
        {
            To = email,
            Subject = "Hoş Geldiniz!",
            Body = "<h1>Kaydınız başarıyla tamamlandı.</h1>"
        };
        string messageJson = JsonSerializer.Serialize(emailMessage);
        _messagePublisher.Publish("email-queue", messageJson);

        return Ok("Kullanıcı kaydedildi ve hoş geldiniz e-postası gönderim için kuyruğa alındı.");
    }
    [HttpPost("register-delayed")]
    public IActionResult RegisterUserWithDelayedEmail([FromBody] string email)
    {

        var emailMessage = new EmailMessage
        {
            To ="karsezer61@gmail.com",
            Subject = "Hatırlatma: Hoş Geldiniz!",
            Body = "<h1>1 dakika önce kaydolmuştunuz!</h1>"
        };
        
        string messageJson = JsonSerializer.Serialize(emailMessage);
        var delay = TimeSpan.FromMinutes(1);
        _messagePublisher.PublishDelayed("email-queue", messageJson, delay);

        return Ok("Kullanıcı kaydedildi. Hoş geldiniz e-postası 1 dakika sonra gönderilmek üzere zamanlandı.");
    }
}

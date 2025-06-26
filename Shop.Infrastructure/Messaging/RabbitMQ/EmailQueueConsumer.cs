namespace Shop.Infrastructure.Messaging.RabbitMQ;

using System.Text;
using System.Text.Json;
using global::RabbitMQ.Client;
using global::RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Domain.Models.Messaging;
using Shop.Domain.Models.Messaging.Smtp;

public class EmailQueueConsumer : BackgroundService
{
    private readonly ILogger<EmailQueueConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "email-queue"; 
    public EmailQueueConsumer(IOptions<RabbitMQSettings> rabbitMqOptions, ILogger<EmailQueueConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqOptions.Value.HostName,
            DispatchConsumersAsync = true 
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
       

    }
  
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        // Bu ayar, RabbitMQ'nun bir seferde sadece 1 mesaj göndermesini sağlar.
        // İşlem bitmeden yenisini göndermez.
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            try
            {
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(messageJson);
                _logger.LogInformation($"E-posta mesajı kuyruktan alındı: {emailMessage.To}");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    await emailService.SendEmailAsync(emailMessage.To, emailMessage.Subject, emailMessage.Body);
                }

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                _logger.LogInformation($"E-posta başarıyla gönderildi: {emailMessage.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"E-posta gönderilirken hata oluştu: {messageJson}");
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        _logger.LogInformation("E-posta kuyruğu dinlenmeye başlandı.");
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
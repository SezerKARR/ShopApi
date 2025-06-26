namespace Shop.Infrastructure.Messaging.RabbitMQ;

using System.Text;
using global::RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Shop.Domain.Models.Messaging;


public class MessagePublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel; // 'IModel' artık tanınmalı
    private const string DelayedExchangeName = "delayed-exchange";

    public MessagePublisher(IOptions<RabbitMQSettings> options)
    {
        var settings = options.Value;
        var factory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password,
            VirtualHost = settings.VirtualHost,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        // 'CreateConnection' artık tanınmalı
        _connection = factory.CreateConnection(); 
        _channel = _connection.CreateModel();
        var args = new Dictionary<string, object> { { "x-delayed-type", "direct" } };

        _channel.ExchangeDeclare(DelayedExchangeName, "x-delayed-message", true, false, args);
    }
    public void PublishDelayed(string queueName, string message, TimeSpan delay)
    {
        // Kuyruğu deklare et
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        // Kuyruğu gecikmeli exchange'e bağla
        _channel.QueueBind(queue: queueName, exchange: DelayedExchangeName, routingKey: queueName);

        var body = Encoding.UTF8.GetBytes(message);
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        
        // Gecikme süresini milisaniye olarak header'a ekle
        properties.Headers = new Dictionary<string, object>
        {
            { "x-delay", (int)delay.TotalMilliseconds }
        };

        _channel.BasicPublish(
        exchange: DelayedExchangeName,
        routingKey: queueName, // Bu routing key ile hangi kuyruğa gideceğini belirler
        basicProperties: properties,
        body: body);
    }
    public void Publish(string queueName, string message)
    {
        _channel.QueueDeclare(queue: queueName,
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(exchange: "",
        routingKey: queueName,
        basicProperties: properties,
        body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
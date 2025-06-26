namespace Shop.Domain.Models.Messaging;

public interface IMessagePublisher
{
    void Publish(string queueName, string message);
    void PublishDelayed(string queueName, string message, TimeSpan delay);
}
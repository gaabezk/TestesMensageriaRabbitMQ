using MediatR;

namespace Application.Commands;

public class PublishMessageToQueueCommand : IRequest<bool>
{
    public PublishMessageToQueueCommand(string message, string queueName)
    {
        Message = message;
        QueueName = queueName;
    }

    public string Message { get; set; }
    public string QueueName { get; set; }
}
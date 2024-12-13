using Application.Commands;
using Domain;
using Domain.Services;
using MediatR;

namespace Application.Handlers;

public class PublishMessageToQueueCommandHandler : IRequestHandler<PublishMessageToQueueCommand, bool>
{
    private readonly IRabbitMqService _rabbitMqService;

    public PublishMessageToQueueCommandHandler(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public async Task<bool> Handle(PublishMessageToQueueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _rabbitMqService.PublishMessageAsync(request.Message, request.QueueName, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
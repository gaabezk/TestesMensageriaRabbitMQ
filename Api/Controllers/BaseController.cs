
using Application.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    [HttpGet("[action]")]
    public IActionResult Init()
    {
        return Ok("Hello World!");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult>  PostMessage([FromQuery] string message, [FromQuery] string queueName, [FromServices] IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new PublishMessageToQueueCommand(message, queueName), cancellationToken);
        return Ok();
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult>  PostProductMessage([FromBody] Product product, [FromQuery] string queueName, [FromServices] IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            var message = JsonConvert.SerializeObject(product);
            await mediator.Send(new PublishMessageToQueueCommand(message, queueName), cancellationToken);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController
{
    private readonly ILogger<PostController> _logger;
    private readonly ICommandDispatcher _dispatcher;

    public PostController(ILogger<PostController> logger, ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> NewPostCommand(NewPostCommand command)
    {
        try
        {
            var id = Guid.NewGuid();
            command.Id = id;
            await _dispatcher.SendAsync(command);
            return new CreatedResult("/", new { });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Client made a bad request");
            return new BadRequestResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
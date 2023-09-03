using CQRS.Core.Handlers;
using Post.Cmd.Core.Aggregates;
using Post.Cmd.Infrastructure.Handlers;

namespace Post.Cmd.Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }


    public async Task HandleAsync(NewPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Auther, command.Message);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditMessageCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.EditMessage(command.Message);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.LikePsot();
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.AddComment(command.Comment, command.Username);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.Username);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.Username);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByAggregateIdAsync(command.Id);
        aggregate.DeletePost(command.Username);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}
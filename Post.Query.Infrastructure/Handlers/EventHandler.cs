using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public EventHandler(IPostRepository postRepository , ICommentRepository commentRepository )
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostEntity
        {
            PostId = @event.Id,
            Auther = @event.Auther,
            DatePosted = @event.DatePosted,
            Message = @event.Message,
        };

       await _postRepository.CreateAsync(post);
    }

    public async Task On(MessageEditedEvent @event)
    {
        var post = await _postRepository.getByIdAsync(@event.Id);
        if(post == null) return;
        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.getByIdAsync(@event.Id);
        if(post == null) return;
        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }
    
    public async Task On(CommentAddedEvent @event)
    {
        var comment = new CommentEntity
        {
            CommentId = @event.CommentId,
            Username = @event.Username,
            CommentDate = @event.CommentDate,
            Comment = @event.Comment,
            PostId = @event.Id
        };
        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment =await _commentRepository.getByIdAsync(@event.CommentId);
        if(comment == null) return;
        comment.Comment = @event.Comment;
        comment.Edited =true;
        comment.CommentDate =@event.EditDate;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }
    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.PostId);
    }
}
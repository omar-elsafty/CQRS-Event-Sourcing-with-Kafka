using Amazon.Runtime.Internal.Transform;
using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Core.Aggregates;

public  class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _auther;

    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public PostAggregate()
    {
    }

    public PostAggregate(Guid id, string auther, string message)
    {
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            Auther = auther,
            Message = message,
            DatePosted = DateTime.UtcNow
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _auther = @event.Auther;
        _active = true;
    }


    public void EditMessage(string message)
    {
        if (!_active)
            throw new Exception("YOu cannot edit message of an inactive post.");

        RaiseEvent(new MessageEditedEvent
        {
            Id = _id,
            Message = message
        });
    }

    public void Apply(MessageEditedEvent @event)
    {
        _id = @event.Id;
    }

    public void LikePsot()
    {
        if (!_active)
            throw new Exception("YOu cannot like an inactive post.");

        RaiseEvent(new PostLikedEvent()
        {
            Id = _id,
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string username)
    {
        if (!_active)
            throw new Exception("YOu cannot edit message of an inactive post.");

        RaiseEvent(new CommentAddedEvent
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.UtcNow
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    }

    public void EditComment(Guid commentId, string comment, string username)
    {
        if (!_active)
            throw new Exception("You cannot edit comment of an inactive post.");
        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("You are not allowed to edit a comment that was made by another user");

        RaiseEvent(new CommentUpdatedEvent()
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            Username = username,
            EditDate = DateTime.UtcNow
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = (new Tuple<string, string>(@event.Comment, @event.Username));
    }

    public void RemoveComment(Guid commentId, string username)
    {
        if (!_active)
            throw new Exception("You cannot edit remove of an inactive post.");

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("You are not allowed to remove a comment that was made by another user");

        RaiseEvent(new CommentRemovedEvent()
        {
            Id = _id,
            CommentId = commentId,
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }


    public void DeletePost(string username)
    {
        if (!_active)
            throw new Exception("You cannot delete an inactive post.");

        if (!_auther.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("You are not allowed to remove a comment that was made by another user");

        RaiseEvent(new PostRemovedEvent()
        {
            Id = _id,
        });
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}
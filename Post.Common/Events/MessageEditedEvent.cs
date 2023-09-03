using CQRS.Core.Events;

namespace Post.Common.Events;

public class MessageEditedEvent : BaseEvent
{
    public MessageEditedEvent() : base(nameof(MessageEditedEvent))
    {
    }

    public string Message { get; set; }  
}
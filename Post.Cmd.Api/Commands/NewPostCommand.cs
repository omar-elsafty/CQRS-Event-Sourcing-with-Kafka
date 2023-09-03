using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class NewPostCommand :BaseCommand
{
    public string Auther { get; set; }   
    public string Message { get; set; }   
}
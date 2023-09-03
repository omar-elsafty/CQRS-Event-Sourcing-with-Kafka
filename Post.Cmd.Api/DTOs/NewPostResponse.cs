using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs;

public class NewPostResponse : BaseResponse
{
    public Guid PostId { get; set; }
}
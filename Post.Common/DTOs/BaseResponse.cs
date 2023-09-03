namespace Post.Common.DTOs;

public class BaseResponse
{
    public BaseResponse()
    {
            
    }
    public BaseResponse(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}
namespace CleanBlazor.Contracts.Chat;

public class ChatMessageResponse
{
    public long Id { get; set; }
    public string FromUserId { get; set; }
    public string FromUserImageUrl { get; set; }
    public string FromUserFullName { get; set; }
    public string ToUserId { get; set; }
    public string ToUserImageUrl { get; set; }
    public string ToUserFullName { get; set; }
    public string Message { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

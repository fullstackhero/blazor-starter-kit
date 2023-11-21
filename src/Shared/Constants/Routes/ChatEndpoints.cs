namespace CleanBlazor.Shared.Constants.Routes;

public static class ChatEndpoint
{
    private const string Prefix = "api/v1/chats";

    public const string GetAvailableUsers = $"{Prefix}/users";
    public const string SaveMessage = Prefix;

    public static string GetChatHistory(string userId) => $"{Prefix}/{userId}";
}

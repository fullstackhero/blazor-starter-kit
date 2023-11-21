namespace CleanBlazor.Shared.Constants.Storage;

public static class StorageConstants
{
    public static class Local
    {
        public const string Preference = "clientPreference";

        public const string AuthToken = "authToken";
        public const string RefreshToken = "refreshToken";
        public const string UserImageUrl = "userImageURL";
    }

    public static class Server
    {
        public const string Preference = "serverPreference";

        //TODO - add
    }
}

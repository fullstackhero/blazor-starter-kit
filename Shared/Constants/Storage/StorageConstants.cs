namespace BlazorHero.CleanArchitecture.Shared.Constants.Storage
{
    public static class StorageConstants
    {
        public static class Local
        {
            public static string Preference = "clientPreference";

            public static string AuthToken = "authToken";
            public static string RefreshToken = "refreshToken";
            public static string UserImageURL = "userImageURL";
        }

        public static class Server
        {
            public static string Preference = "serverPreference";

            //TODO - add
        }
    }
}
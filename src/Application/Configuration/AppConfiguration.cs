namespace CleanBlazor.Application.Configuration;

public class AppConfiguration
{
    public string Secret { get; set; }

    public bool BehindSslProxy { get; set; }

    public string ProxyIp { get; set; }

    public string ApplicationUrl { get; set; }
}

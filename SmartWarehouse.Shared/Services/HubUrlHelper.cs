using Microsoft.AspNetCore.Components;

namespace SmartWarehouse.Shared.Services;

public static class HubUrlHelper
{
    private const string LocalhostIp = "127.0.0.1";
    private const string ZeroAddress = "0.0.0.0";
    private const string HttpsScheme = "https";
    private const int DefaultHttpsPort = 7229;
    private const int DefaultHttpPort = 5055;

    public static string GetHubUrl(NavigationManager nav, string hubPath)
    {
        var baseUri = new Uri(nav.BaseUri);
        var host = baseUri.Host;

        if (IsZeroAddress(host) || IsLocalhostAddress(host))
        {
            return BuildLocalhostUrl(baseUri, hubPath);
        }

        return nav.ToAbsoluteUri(hubPath).ToString();
    }

    private static bool IsLocalhostAddress(string host)
    {
        return host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
               host.Equals(LocalhostIp, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsZeroAddress(string host)
    {
        return host.Equals(ZeroAddress, StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildLocalhostUrl(Uri baseUri, string hubPath)
    {
        var scheme = baseUri.Scheme;
        var port = baseUri.Port > 0 ? baseUri.Port : GetDefaultPort(scheme);
        return $"{scheme}://{LocalhostIp}:{port}{hubPath}";
    }

    private static int GetDefaultPort(string scheme)
    {
        return scheme == HttpsScheme ? DefaultHttpsPort : DefaultHttpPort;
    }
}
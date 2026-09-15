using ConsoleTables;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace https_check;

internal class HttpExecute
{
    internal static void Run()
    {
        Uri? uri = null;
        {
            var table = new ConsoleTable(new ConsoleTableOptions { Columns = ["Name", "Value"], EnableCount = false });
            if (TryGetUri(CommandLineOptions.Options.Url, out uri))
            {
                table.AddRow("URL", CommandLineOptions.Options.Url);
                table.AddRow("Host", uri!.Host);
                table.AddRow("Port", uri!.Port);
                IPAddress? address;
                if (!IPAddress.TryParse(uri.Host, out address))
                {
                    if (TryGetIPAddress(uri.Host, out var iPAddresses))
                    {
                        address = iPAddresses!.First();
                    }
                }
                table.AddRow("IP Address", address != null ? address.ToString() : "Unknown");
                if (address != null)
                {
                    try
                    {
                        using TcpClient tcpClient = new();
                        tcpClient.Connect(uri.Host, uri.Port);
                        using var stream = tcpClient.GetStream();
                        using var sslStream = new SslStream(stream, false, (sender, certificate, chain, sslPolicyErrors) => true);
#pragma warning disable CS0618 // 类型或成员已过时
                        sslStream.AuthenticateAsClient(uri.Host, null, SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13, false);
#pragma warning restore CS0618 // 类型或成员已过时
                        var cert = new X509Certificate2(sslStream.RemoteCertificate!);
                        table.AddRow("Domain Name", cert.GetNameInfo(X509NameType.SimpleName, false));
                        table.AddRow("Issuer", cert.Issuer);
                        table.AddRow("Certificate Start Date", cert.NotBefore);
                        table.AddRow("Certificate Expiration Date", cert.NotAfter);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                table.Write();
            }
            else
            {
                Console.WriteLine("The URL address entered is incorrect");
                return;
            }
        }
        {
            // Test the supported security protocols
            var table = new ConsoleTable(new ConsoleTableOptions { Columns = ["Security Protocol", "Result"], EnableCount = false });
            foreach (var item in Enum.GetValues(typeof(SslProtocols)))
            {
                if (item is SslProtocols.None) continue;
                var result = TcpConnect(uri, (SslProtocols)item);
                table.AddRow(item.ToString()!, result ? "YES" : "NO");
            }
            table.Write();
        }
        {
            // Test the supported HTTP versions
            var table = new ConsoleTable(new ConsoleTableOptions { Columns = ["Http Version", "Result"], EnableCount = false });
            using var httpclient = new HttpClient();
            foreach (var item in new[] { System.Net.HttpVersion.Version10, 
                System.Net.HttpVersion.Version11,
                System.Net.HttpVersion.Version20,
                System.Net.HttpVersion.Version30})
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, CommandLineOptions.Options.Url);
                    request.Version = item;
                    request.VersionPolicy = HttpVersionPolicy.RequestVersionExact;
                    _ = httpclient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
                    table.AddRow(item.ToString()!, "YES");
                }
                catch 
                {
                    table.AddRow(item.ToString()!, "NO");
                }
            }
            table.Write();
        }
    }

    /// <summary>
    /// Attempt to convert URL address 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    private static bool TryGetUri(string url, out Uri? uri)
    {
        try
        {
            uri = new(url);
            return true;
        }
        catch
        {
            uri = null;
            return false;
        }
    }

    /// <summary>
    /// Get IP Address by host
    /// </summary>
    /// <param name="host"></param>
    /// <param name="iPAddresses"></param>
    /// <returns></returns>
    private static bool TryGetIPAddress(string host, out IPAddress[]? iPAddresses)
    {
        try
        {
            iPAddresses = Dns.GetHostAddresses(host);
            return true;
        }
        catch
        {
            iPAddresses = null;
            return false;
        }
    }

    /// <summary>
    /// Send HTTP request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="securityProtocolType"></param>
    /// <returns></returns>
    [Obsolete("This method is obsolete. Use an alternative method.")]
    private static bool DoHttps(string url, SecurityProtocolType securityProtocolType)
    {
        var result = true;
        try
        {
            ServicePointManager.SecurityProtocol = securityProtocolType;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = CommandLineOptions.Options.Timeout;
            request.KeepAlive = false;
            _ = request.GetResponse();
        }
        catch (WebException webEx) when (webEx.Response is not null) { }
        catch
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// Attempting to establish a connection using the TCP protocol
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="address"></param>
    /// <param name="sslProtocols"></param>
    /// <returns></returns>
    public static bool TcpConnect(Uri uri, SslProtocols sslProtocols)
    {
        try
        {
            using TcpClient tcpClient = new();
            tcpClient.Connect(uri.Host, uri.Port);
            using var stream = tcpClient.GetStream();
            // 根据 CommandLineOptions.Options.Check 决定是否校验证书
            static bool validationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
            {
                if (CommandLineOptions.Options.Check)
                {
                    // 仅当没有策略错误时视为有效
                    return sslPolicyErrors == SslPolicyErrors.None;
                }
                // 不校验证书时始终返回 true
                return true;
            }
            using var sslStream = new SslStream(stream, false, validationCallback);
            sslStream.AuthenticateAsClient(uri.Host, null, sslProtocols, false);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

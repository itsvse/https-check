using ConsoleTables;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace https_check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            Console.WriteLine(@" _   _ _   _                ____ _               _    
| | | | |_| |_ _ __  ___   / ___| |__   ___  ___| | __
| |_| | __| __| '_ \/ __| | |   | '_ \ / _ \/ __| |/ /
|  _  | |_| |_| |_) \__ \ | |___| | | |  __/ (__|   < 
|_| |_|\__|\__| .__/|___/  \____|_| |_|\___|\___|_|\_\
              |_|                                     
");
            Console.WriteLine($"Version: {Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion}");
            if (!args.Any() || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("Please enter the URL in the argument.");
                return;
            }
            var url = args[0];
            if (!url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("URL must start with https.");
                return;
            }
            IPAddress? address;
            Uri? uri;
            bool connected = false;
            {
                var table = new ConsoleTable(new ConsoleTableOptions { Columns = new List<string>() { "Name", "Value" }, EnableCount = false });
                if (TryGetUri(url, out uri))
                {
                    table.AddRow("URL", url);
                    table.AddRow("Host", uri!.Host);
                    table.AddRow("Port", uri!.Port);
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
                            tcpClient.Connect(address, uri.Port);
                            using var stream = tcpClient.GetStream();
                            using var sslStream = new SslStream(stream, false, (sender, certificate, chain, sslPolicyErrors) => true);
#pragma warning disable CS0618 // 类型或成员已过时
#pragma warning disable SYSLIB0039 // 类型或成员已过时
                            var enabledSslProtocols = 
                                SslProtocols.Ssl2 |
                                 SslProtocols.Ssl3 |
                                 SslProtocols.Tls |
                                 SslProtocols.Tls11 |
                                 SslProtocols.Tls12 |
                                 SslProtocols.Tls13;
#pragma warning restore SYSLIB0039 // 类型或成员已过时
#pragma warning restore CS0618 // 类型或成员已过时
                            sslStream.AuthenticateAsClient(uri.Host, null, enabledSslProtocols, false);
                            var cert = new X509Certificate2(sslStream.RemoteCertificate!);
                            table.AddRow("Domain Name", cert.GetNameInfo(X509NameType.SimpleName, false));
                            table.AddRow("Issuer", cert.Issuer);
                            table.AddRow("Certificate Start Date", cert.NotBefore);
                            table.AddRow("Certificate Expiration Date", cert.NotAfter);
                            connected = true;
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
                var table = new ConsoleTable(new ConsoleTableOptions { Columns = new List<string>() { "Security Protocol", "Result" }, EnableCount = false });
                foreach (var item in Enum.GetValues(typeof(SslProtocols)))
                {
                    if (item is SslProtocols.None) continue;
                    if (connected)
                    {
                        var result = TcpConnect(uri!, address!, (SslProtocols)item);
                        table.AddRow(item.ToString()!, result ? "YES" : "NO");
                    }
                    else {
                        table.AddRow(item.ToString()!, "Unknown");
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
        /// Attempting to establish a connection using the TCP protocol
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="address"></param>
        /// <param name="sslProtocols"></param>
        /// <returns></returns>
        public static bool TcpConnect(Uri uri, IPAddress address, SslProtocols sslProtocols)
        {
            try
            {
                if (sslProtocols is SslProtocols.Tls13)
                { 
                    
                }
                using TcpClient tcpClient = new();
                tcpClient.Connect(address, uri.Port);
                using var stream = tcpClient.GetStream();
                using var sslStream = new SslStream(stream, false, (sender, certificate, chain, sslPolicyErrors) => true);
                sslStream.AuthenticateAsClient(uri.Host, null, sslProtocols, false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
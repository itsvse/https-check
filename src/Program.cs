﻿using ConsoleTables;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
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
            {
                var table = new ConsoleTable(new ConsoleTableOptions { Columns = new List<string>() { "Name", "Value" }, EnableCount = false });
                if (TryGetUri(url, out var uri))
                {
                    table.AddRow("URL", url);
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
                            tcpClient.Connect(address, uri.Port);
                            using var stream = tcpClient.GetStream();
                            using var sslStream = new SslStream(stream, false, (sender, certificate, chain, sslPolicyErrors) => true);
                            sslStream.AuthenticateAsClient(uri.Host);
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
                var table = new ConsoleTable(new ConsoleTableOptions { Columns = new List<string>() { "Security Protocol", "Result" }, EnableCount = false });
                foreach (var item in Enum.GetValues(typeof(SecurityProtocolType)))
                {
                    var result = DoHttps(url, (SecurityProtocolType)item);
                    table.AddRow(item.ToString()!, result ? "YES" : "NO");
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
        private static bool DoHttps(string url, SecurityProtocolType securityProtocolType)
        {
            var result = true;
            try
            {
                ServicePointManager.SecurityProtocol = securityProtocolType;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
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

    }
}
using ConsoleTables;
using System.Net;

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
            Console.WriteLine($"Test URL: {url}");
            Console.WriteLine();
            var table = new ConsoleTable(new ConsoleTableOptions { Columns = new List<string>() { "Security Protocol", "Result" }, EnableCount = false });
            foreach (var item in Enum.GetValues(typeof(SecurityProtocolType)))
            {
                var result = DoHttps(url, (SecurityProtocolType)item);
                table.AddRow(item.ToString()!, result ? "YES" : "NO");
            }
            table.Write();
        }

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
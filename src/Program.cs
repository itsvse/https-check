using McMaster.Extensions.CommandLineUtils;

namespace https_check
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine(@" _   _ _   _                ____ _               _    
| | | | |_| |_ _ __  ___   / ___| |__   ___  ___| | __
| |_| | __| __| '_ \/ __| | |   | '_ \ / _ \/ __| |/ /
|  _  | |_| |_| |_) \__ \ | |___| | | |  __/ (__|   < 
|_| |_|\__|\__| .__/|___/  \____|_| |_|\___|\___|_|\_\
              |_|                                     
");
            Console.WriteLine($"Version: {CommandLineOptions.GetVersion()}");
            return CommandLineApplication.Execute<CommandLineOptions>(args);
        }
    }
}
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace https_check
{
    /// <summary>
    /// Command Line Options
    /// </summary>
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    internal class CommandLineOptions
    {
        internal static CommandLineOptions Options { get; private set; } = new CommandLineOptions();

        [Argument(0)]
        [Required]
        public string Url { get; } = null!;

        [Option("-t|--timeout", Description = "Set HTTP request timeout time (in milliseconds)")]
        public int Timeout { get; } = 10_000;

        [Option("-c|--check", Description = "Enable HTTPS certificate checking")]
        public bool Check { get; }

        /// <summary>
        /// Execute Method
        /// </summary>
        private void OnExecute()
        {
            if (!Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("URL must start with https.");
                return;
            }
            Options = this;
            HttpExecute.Run();
        }

        internal static string GetVersion()
        => typeof(CommandLineOptions).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}

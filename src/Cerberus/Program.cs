using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus
{
    using System.Diagnostics;
    using Core.Configuration;
    using NDesk.Options;

    class Program
    {
        static int Main(string[] args)
        {
            PrintLogo();
            var configFilePath = "";
            
            configFilePath = GetConfigFilePath(args);
            if (string.IsNullOrEmpty(configFilePath))
            {
                ShowHelp();
                return -1;
            }

            if (!string.IsNullOrEmpty(configFilePath))
            {
                var stopwatch = Stopwatch.StartNew();
                var cerberus = new ConfigInitializer(configFilePath);
                var analyzeResults = cerberus.HelixAnalyzerService.Analyze();
                stopwatch.Stop();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Analyzing took: {stopwatch.Elapsed.Seconds} seconds");
                Console.WriteLine($"Count of applied Rules: {analyzeResults.Results.Count}");
                var returnCode = cerberus.ExitCodePolicyService.GetExitCodePolicy(analyzeResults);
                Console.WriteLine($"Exit code is: {returnCode}");
                return returnCode;
            }

            return -1;
        }

        private static string GetConfigFilePath(string[] args)
        {
            string configFilePath = "";
            var optionSet = new OptionSet()
            {
                {
                    "c|config=", "path to the cerberus.config.",
                    v => configFilePath = v
                },
                {
                    "h|help", "show help.",
                    v => ShowHelp()
                },
            };
            try
            {
                optionSet.Parse(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return configFilePath;
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t c|config \tspecify the path to cerberus.config. ");
            Console.WriteLine("\t h|help   \tshow this help. ");
            Console.ResetColor();
        }

        private static void PrintLogo()
        {
            string s = Properties.Settings.Default.Cerberus;
            Console.ForegroundColor = ConsoleColor.Red;
            var z = Console.CursorSize;
            Console.CursorSize = 1;
            Console.WriteLine(s);
            Console.ResetColor();

        }
    }
}

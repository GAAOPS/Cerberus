namespace Cerberus
{
    using System;
    using System.Diagnostics;
    using Core.Configuration;
    using NDesk.Options;
    using Properties;

    internal static class Program
    {
        private static int Main(string[] args)
        {
            PrintLogo();
            var configFilePath = "";

            configFilePath = GetConfigFilePath(args);
            if (string.IsNullOrEmpty(configFilePath))
            {
                ShowHelp();

                return -1;
            }

            var stopwatch = Stopwatch.StartNew();
            var cerberus = new ConfigInitializer(configFilePath);
            var analyzeResults = cerberus.HelixAnalyzerService.Analyze();
            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Analyzing took: {stopwatch.Elapsed.Seconds} seconds");
            Console.WriteLine($"Count of applied Rules: {analyzeResults.Results.Count}");
            var returnCode = cerberus.ExitCodePolicyService.GetExitCodePolicy(analyzeResults);
            Console.WriteLine($"Exit code is: {returnCode}");
            Console.ResetColor();

            return returnCode;
        }

        private static string GetConfigFilePath(string[] args)
        {
            var configFilePath = "";
            var optionSet = new OptionSet
            {
                {
                    "c|config=", "path to the cerberus.config.",
                    v => configFilePath = v
                },
                {
                    "h|help", "show help.",
                    v => ShowHelp()
                }
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
            var s = Settings.Default.Cerberus;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}
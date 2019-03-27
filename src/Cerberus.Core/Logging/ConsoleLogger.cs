namespace Cerberus.Core.Logging
{
    using System;

    public class ConsoleLogger : Logger, IConsoleLogger
    {
        public override void Log(string message, LogLevel log)
        {
            switch (log)
            {
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
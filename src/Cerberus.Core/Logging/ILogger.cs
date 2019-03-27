namespace Cerberus.Core.Logging
{
    public interface ILogger
    {
        void Log(string message, LogLevel log);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Debug(string message);
        void Report();
        void InitLogger(string name, string description);
    }
}
namespace Cerberus.Core.Logging
{
    public abstract class Logger : ILogger
    {
        public abstract void Log(string message, LogLevel log);

        public void Info(string message)
        {
            Log(message, LogLevel.Information);
        }

        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public virtual void Report()
        {
        }

        public virtual void InitLogger(string name, string description)
        {
        }
    }
}
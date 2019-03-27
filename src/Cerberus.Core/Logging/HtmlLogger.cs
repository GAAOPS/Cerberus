namespace Cerberus.Core.Logging
{
    using System.IO;
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using Configuration;

    public class HtmlLogger : Logger, IHtmlLogger
    {
        private readonly ExtentReports _extent;
        private ExtentTest _test;

        public HtmlLogger(string outputPath, IDataSourceLocation dataSourceLocation)
        {
            var path = Path.GetFullPath(outputPath.ToLower().Replace("$(configpath)", dataSourceLocation.Root));
            path = path.EndsWith(@"\") ? path : $@"{path}\";

            var reporter = new ExtentHtmlReporter(path);

            // create ExtentReports and attach reporter(s)
            _extent = new ExtentReports();
            _extent.AttachReporter(reporter);
        }

        public override void InitLogger(string name, string description)
        {
            _test = _extent.CreateTest(name, description);
        }

        public override void Log(string message, LogLevel log)
        {
            Status status;
            switch (log)
            {
                case LogLevel.Information:
                    status = Status.Pass;
                    break;
                case LogLevel.Warning:
                    status = Status.Warning;
                    break;
                case LogLevel.Error:
                    status = Status.Fail;
                    break;
                case LogLevel.Debug:
                    status = Status.Debug;
                    break;
                case LogLevel.None:
                    status = Status.Info;
                    break;
                default:
                    status = Status.Info;
                    break;
            }

            _test.Log(status, message);
        }

        public override void Report()
        {
            Flush();
        }

        private void Flush()
        {
            _extent.Flush();
        }
    }
}
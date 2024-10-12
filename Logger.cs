public static void LoggerConfiguration()
{
    var hierarchy = (Hierarchy)LogManager.GetRepository();

    var layout = new log4net.Layout.PatternLayout
    {
        ConversionPattern = "%date %-5level %logger - %message%newline"
    };
    layout.ActivateOptions();


    var rollingFileAppender1 = new RollingFileAppender
    {
        File = "Mainlogger.txt",
        AppendToFile = false,
        RollingStyle = RollingFileAppender.RollingMode.Size,
        MaximumFileSize = "1MB",
        MaxSizeRollBackups = 5,
        StaticLogFileName = true,
        Layout = layout,
    };
    rollingFileAppender1.ActivateOptions();

    // Appender for Class2
    var rollingFileAppender2 = new RollingFileAppender
    {
        File = "Errorlogger.txt",
        AppendToFile = false,
        RollingStyle = RollingFileAppender.RollingMode.Size,
        MaximumFileSize = "1MB",
        MaxSizeRollBackups = 5,
        StaticLogFileName = true,
        Layout = layout
    };
    rollingFileAppender2.ActivateOptions();

    var class1Logger = LogManager.GetLogger(typeof(ConsoleApp5.Program));
    var class2Logger = LogManager.GetLogger(typeof(ConsoleApp4.Program));

    ((log4net.Repository.Hierarchy.Logger)class1Logger.Logger).AddAppender(rollingFileAppender1);
    ((log4net.Repository.Hierarchy.Logger)class2Logger.Logger).AddAppender(rollingFileAppender2);

    hierarchy.Root.Level = log4net.Core.Level.Info;
    hierarchy.Configured = true;
}

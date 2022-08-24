using NLog;

public class LoggerManager : ILoggerManager
{
    private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();
    public void LogDebug(string message) => logger.Debug(message);
    public void LogInfo(string message) => logger.Info(message);    
    public void LogWarn(string message) => logger.Warn(message);    
    public void LogError(Exception ex, string message) => logger.Error(ex, message);
}
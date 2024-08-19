namespace LoggingExample.Models;

public class FileLogger
{
    private readonly string _logFilePath;
    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;

        var directory = Path.GetDirectoryName(_logFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
    public async Task LogEventAsync(string message, string logLevel)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}]{message}";
        await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
    }
}

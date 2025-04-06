
namespace WhiteRabbit.TauriPlugIn;

public class FileLogger : ILogger
{
    private readonly string myLogPath;

    public FileLogger(AppDataRoot root)
    {
        myLogPath = Path.Combine(root.Location, "app.log");
    }

    public void Error(string message)
    {
        File.AppendAllText(myLogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|ERROR|{message}" + Environment.NewLine);
    }

    public void Info(string message)
    {
        File.AppendAllText(myLogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|INFO|{message}" + Environment.NewLine);
    }

    public void Warning(string message)
    {
        File.AppendAllText(myLogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|WARNING|{message}" + Environment.NewLine);
    }
}
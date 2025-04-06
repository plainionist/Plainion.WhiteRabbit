using System.Diagnostics;
using Newtonsoft.Json;

namespace WhiteRabbit.TauriPlugIn;

public class DataStore
{
    private readonly ILogger myLogger;
    private readonly string myDBStore;

    public DataStore(ILogger logger, AppDataRoot root)
    {
        myLogger = logger;
        myDBStore = root.Location;
    }

    public IReadOnlyCollection<Activity> GetActivities(DateTime date)
    {
        var file = GetFile(date);
        return File.Exists(file)
            ? JsonConvert.DeserializeObject<List<Activity>>(File.ReadAllText(file)) ?? []
            : [];
    }

    private string GetFile(DateTime date) =>
        Path.Combine(myDBStore, $"{date:yyyy-MM-dd}.json");

    public void Update(DateTime date, IEnumerable<Activity> activities)
    {
        File.WriteAllText(GetFile(date), JsonConvert.SerializeObject(activities, Formatting.Indented));

        CommitOnDemand();
    }

    private void CommitOnDemand()
    {
        if (!Directory.Exists(Path.Combine(myDBStore, ".git")))
        {
            myLogger.Warning("No git repository found. Skipping commit.");
            return;
        }

        var git = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "commit -am \".\"",
                WorkingDirectory = myDBStore,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };

        void LogOutput(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                myLogger.Info(e.Data);
            }
        }

        git.OutputDataReceived += LogOutput;
        git.ErrorDataReceived += LogOutput;

        void OnExit(object? sender, EventArgs e)
        {
            git.OutputDataReceived -= LogOutput;
            git.ErrorDataReceived -= LogOutput;
            git.Exited -= OnExit;

            myLogger.Info($"Git process exited with code {git.ExitCode}");
            git.Dispose();
        }

        git.Exited += OnExit;

        git.Start();
        git.BeginOutputReadLine();
        git.BeginErrorReadLine();

    }

 private void RunGitCommand(string args)
    {
        var git = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = args,
                WorkingDirectory = myDBStore,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };

        git.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                myLogger.Info($"[git stdout] {e.Data}");
        };

        git.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                myLogger.Error($"[git stderr] {e.Data}");
        };

        git.Exited += (sender, e) =>
        {
            myLogger.Info($"Git process '{args}' exited with code {git.ExitCode}");
            git.Dispose();
        };

        git.Start();
        git.BeginOutputReadLine();
        git.BeginErrorReadLine();
    }

}

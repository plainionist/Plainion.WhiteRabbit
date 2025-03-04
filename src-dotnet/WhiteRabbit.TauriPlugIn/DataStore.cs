using System.Diagnostics;
using Newtonsoft.Json;

namespace WhiteRabbit.TauriPlugIn;

public class DataStore
{
    private static readonly string DBStore = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WhiteRabbit-3.0");

    public DataStore()
    {
        if (!Directory.Exists(DBStore))
        {
            Directory.CreateDirectory(DBStore);
        }
    }

    public IReadOnlyCollection<Activity> GetActivities(DateTime date)
    {
        var file = GetFile(date);
        return File.Exists(file)
            ? JsonConvert.DeserializeObject<List<Activity>>(File.ReadAllText(file)) ?? []
            : [];
    }

    private static string GetFile(DateTime date) =>
        Path.Combine(DBStore, $"{date:yyyy-MM-dd}.json");

    public void Update(DateTime date, IEnumerable<Activity> activities)
    {
        File.WriteAllText(GetFile(date), JsonConvert.SerializeObject(activities, Formatting.Indented));

        CommitOnDemand();
    }

    private void CommitOnDemand()
    {
        if (!Directory.Exists(Path.Combine(DBStore, ".git")))
        {
            return;
        }

        var git = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "commit -am \".\"",
                WorkingDirectory = DBStore,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        git.Start();

        // don' block the UI
        //git.WaitForExit();
    }
}

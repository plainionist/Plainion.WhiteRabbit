namespace WhiteRabbit.TauriPlugIn;

public class AppDataRoot
{
    public AppDataRoot()
    {
        if (!Directory.Exists(Location))
        {
            Directory.CreateDirectory(Location);
        }
    }

    public string Location { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WhiteRabbit-3.0");
}

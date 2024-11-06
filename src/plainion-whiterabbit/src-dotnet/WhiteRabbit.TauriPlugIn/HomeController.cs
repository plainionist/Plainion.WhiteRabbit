namespace WhiteRabbit.TauriPlugIn;

public class DayRequest
{
    public DateTime Date { get; set; }
}

public class UpdateRequest
{
    public DateTime Date { get; set; }
    public Activity[]? Items { get; set; }
}

public class HomeController
{
    private readonly DataStore myDataStore = new();

    public List<Activity> Day(DayRequest request) => myDataStore.Day(request);

    public void AddActivity(Activity activity) => myDataStore.AddActivity(activity);

    public void Update(UpdateRequest request) => myDataStore.Update(request);
}

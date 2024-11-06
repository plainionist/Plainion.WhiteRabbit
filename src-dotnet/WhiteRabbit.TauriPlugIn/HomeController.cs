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

    public IReadOnlyCollection<Activity> Day(DayRequest request) => 
        myDataStore.Day(request.Date);

    public void AddActivity(Activity activity) => 
        myDataStore.AddActivity(activity);

    public void Update(UpdateRequest request)
    {
        var activities = (request.Items ?? [])
            .Where(x => x.Begin != null)
            .ToList();

        myDataStore.Update(request.Date, activities);
    }
}

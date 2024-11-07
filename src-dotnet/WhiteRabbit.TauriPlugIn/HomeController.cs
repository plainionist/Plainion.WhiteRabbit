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

public class HomeController(DataStore dataStore)
{
    private readonly DataStore myDataStore = dataStore;
    
    public IReadOnlyCollection<Activity> Day(DayRequest request) => 
        myDataStore.GetActivities(request.Date);

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

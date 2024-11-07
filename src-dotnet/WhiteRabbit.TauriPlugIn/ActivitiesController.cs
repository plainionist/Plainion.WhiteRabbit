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

public class ActivitiesController(DataStore dataStore)
{
    private readonly DataStore myDataStore = dataStore;

    public IReadOnlyCollection<Activity> Get(DayRequest request) =>
        myDataStore.GetActivities(request.Date);

    public void Update(UpdateRequest request)
    {
        var activities = (request.Items ?? [])
            .Where(x => x.Begin != null)
            .ToList();

        myDataStore.Update(request.Date, activities);
    }
}

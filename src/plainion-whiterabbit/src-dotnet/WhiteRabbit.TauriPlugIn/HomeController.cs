namespace WhiteRabbit.TauriPlugIn;

public class DayRequest
{
    public DateTime Date { get; set; }
}

public class Activity
{
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string? Comment { get; set; }
}

public class UpdateRequest
{
    public DateTime Date { get; set; }
    public Activity[]? Items { get; set; }
}

public class HomeController
{
    private readonly Dictionary<DateTime, List<Activity>> myActivities = [];

    public List<Activity> Day(DayRequest request)
    {
        return myActivities.GetValueOrDefault(request.Date.Date) ?? [];
    }

    public void AddActivity(Activity activity)
    {
        var date = activity.Begin?.Date.Date;
        if (date == null)
        {
            return;
        }

        if (!myActivities.TryGetValue(date.Value, out var activities))
        {
            activities = [];
            myActivities.Add(date.Value, activities);
        }
        activities.Add(activity);
    }

    public void Update(UpdateRequest request)
    {
        myActivities[request.Date.Date] = (request.Items ?? [])
            .Where(x => x.Begin != null)
            .ToList();
    }
}

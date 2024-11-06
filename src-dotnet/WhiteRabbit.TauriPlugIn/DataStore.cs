namespace WhiteRabbit.TauriPlugIn;

public class DataStore
{
    private static readonly string DBStore = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WhiteRabbit-3.0");

    private readonly Dictionary<DateTime, List<Activity>> myActivities = [];

    public IReadOnlyCollection<Activity> Day(DateTime date)
    {
        return myActivities.GetValueOrDefault(date.Date) ?? [];
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

    public void Update(DateTime date, IEnumerable<Activity> activities)
    {
        myActivities[date.Date] = activities.ToList();
    }
}

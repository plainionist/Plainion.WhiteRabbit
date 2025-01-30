namespace WhiteRabbit.TauriPlugIn;

public class ReportRequest
{
    public DateTime Date { get; set; }
}

public class ReportVM
{
    public string? Headline { get; set; }
    public List<ReportGroupVM>? Groups { get; set; }
    public string? Total { get; set; }
}

public class ReportGroupVM
{
    public string? Headline { get; set; }
    public List<ReportEntryVM>? Entries { get; set; }
    public string? Total { get; set; }
}

public class ReportEntryVM
{
    public string? Comment { get; set; }
    public string? Duration { get; set; }
}

public class ReportController(DataStore dataStore)
{
    private readonly DataStore myDataStore = dataStore;

    public ReportVM Day(DayRequest request) =>
        CreateReport([request.Date]);

    private ReportVM CreateReport(IEnumerable<DateTime> days)
    {
        var activities = days.SelectMany(myDataStore.GetActivities)
            .Where(x => x.Duration != null && x.Duration.Value.TotalMinutes > 1)
            .ToList();

        var durationByComment = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase);
        foreach (var activity in activities)
        {
            var comment = string.IsNullOrWhiteSpace(activity.Comment) ? "<empty>" : activity.Comment;

            if (!durationByComment.ContainsKey(comment))
            {
                durationByComment[comment] = activity.Duration!.Value;
            }
            else
            {
                durationByComment[comment] += activity.Duration!.Value;
            }
        }

        var groups = durationByComment
            .GroupBy(x =>
            {
                var tokens = x.Key.Split('/');
                return tokens.Length > 1 ? tokens[0] : "Default";
            }, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new ReportVM
        {
            Headline = days.Count() == 1
                ? $"{days.Single():yyyy-MM-dd}"
                : $"{days.Min():yyyy-MM-dd} - {days.Max():yyyy-MM-dd}",

            Groups = groups
                .Select(group =>
                    new ReportGroupVM
                    {
                        Headline = group.Key,
                        Entries = group
                            .Select(entry => new ReportEntryVM
                            {
                                Comment = entry.Key,
                                Duration = FormatDuration(entry.Value)
                            })
                            .OrderByDescending(entry => entry.Duration)
                            .ToList(),
                        Total = FormatDuration(TimeSpan.FromMinutes(group.Sum(x => x.Value.TotalMinutes)))
                    }
                )
                .ToList(),

            Total = FormatDuration(TimeSpan.FromMinutes(durationByComment.Sum(x => x.Value.TotalMinutes)))
        };
    }

    // support hours > 24
    private string FormatDuration(TimeSpan value) =>
        $"{(int)value.TotalHours:D2}:{value.Minutes:D2}";

    public ReportVM Week(DayRequest request)
    {
        var begin = GetBeginOfWeek(request.Date);
        var end = GetEndOfWeek(request.Date);
        return CreateReport(GetDays(begin, end));
    }

    private static DateTime GetBeginOfWeek(DateTime date)
    {
        while (date.DayOfWeek != DayOfWeek.Monday)
        {
            date = date.AddDays(-1);
        }

        return date;
    }

    private static DateTime GetEndOfWeek(DateTime date)
    {
        while (date.DayOfWeek != DayOfWeek.Sunday)
        {
            date = date.AddDays(1);
        }

        return date;
    }

    private static IEnumerable<DateTime> GetDays(DateTime begin, DateTime end)
    {
        for (var day = begin; day <= end; day = day.AddDays(1))
        {
            yield return day;
        }
    }

    public ReportVM Month(DayRequest request)
    {
        var begin = new DateTime(request.Date.Year, request.Date.Month, 1);
        var end = new DateTime(request.Date.Year, request.Date.Month, DateTime.DaysInMonth(request.Date.Year, request.Date.Month));
        return CreateReport(GetDays(begin, end));
    }
}

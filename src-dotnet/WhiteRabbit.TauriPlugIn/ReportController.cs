namespace WhiteRabbit.TauriPlugIn;

public class ReportRequest
{
    public DateTime Date { get; set; }
}

public class ReportVM
{
    public string? Headline { get; set; }
    public List<ReportEntry>? Entries { get; set; }
    public string? Total { get; set; }
}

public class ReportEntry
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
        var durationByComment = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
        {
            ["<empty>"] = new TimeSpan()
        };

        var total = new TimeSpan();
        foreach (var activity in days.SelectMany(myDataStore.GetActivities))
        {
            var duration = activity.GetDuration();
            if (duration == null)
            {
                continue;
            }

            var comment = string.IsNullOrWhiteSpace(activity.Comment) ? "<empty>" : activity.Comment;

            if (!durationByComment.ContainsKey(comment))
            {
                durationByComment[comment] = duration.Value;
            }
            else
            {
                durationByComment[comment] += duration.Value;
            }

            total += duration.Value;
        }

        return new ReportVM
        {
            Headline = days.Count() == 1
                ? $"{days.Single():yyyy-MM-dd}"
                : $"{days.Min():yyyy-MM-dd} - {days.Max():yyyy-MM-dd}",
            Entries = durationByComment
                .Select(entry => new ReportEntry
                {
                    Comment = entry.Key,
                    Duration = entry.Value.ToString("hh\\:mm")
                })
                .OrderByDescending(entry => entry.Duration)
                .ToList(),
            Total = total.ToString("hh\\:mm")
        };
    }

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


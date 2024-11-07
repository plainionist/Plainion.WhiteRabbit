namespace WhiteRabbit.TauriPlugIn;

public class Activity
{
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string? Comment { get; set; }

    public TimeSpan? GetDuration() =>
        Begin == null || End == null
            ? null
            : GetDuration(Begin.Value, End.Value);

    public static TimeSpan GetDuration(DateTime begin, DateTime end) =>
        end >= begin
            ? end - begin
            : DateTime.Today.AddDays(1) - begin + end.TimeOfDay;
}

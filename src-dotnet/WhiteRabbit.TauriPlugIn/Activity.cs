namespace WhiteRabbit.TauriPlugIn;

public class Activity
{
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string? Comment { get; set; }
    public TimeSpan? Duration =>
        Begin == null || End == null
            ? null
            : End.Value >= Begin.Value 
                ? End.Value - Begin.Value
                : DateTime.Today.AddDays(1) - Begin.Value + End.Value.TimeOfDay;
}

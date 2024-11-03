using System;
using System.Data;

namespace Plainion.WhiteRabbit.Model
{
    public class DayEntry
    {
        public DayEntry()
        {
            Begin = null;
            End = null;
            Comment = null;
        }

        public static DayEntry Parse(DataRow row)
        {
            DayEntry entry = new DayEntry();
            if (row == null)
            {
                return entry;
            }

            if (!row[ColumnNames.BEGIN].IsEmpty())
            {
                entry.Begin = DateTime.Parse((string)row[ColumnNames.BEGIN]);
            }

            if (!row[ColumnNames.END].IsEmpty())
            {
                entry.End = DateTime.Parse((string)row[ColumnNames.END]);
            }

            if (row.Table.Columns.Contains(ColumnNames.COMMENT) && !row[ColumnNames.COMMENT].IsEmpty())
            {
                entry.Comment = (string)row[ColumnNames.COMMENT];
            }

            return entry;
        }

        public TimeSpan? GetUsedTime()
        {
            if (Begin == null || End == null)
            {
                return null;
            }

            return GetDuration(Begin.Value, End.Value);
        }

        public static TimeSpan GetDuration(DateTime begin, DateTime end)
        {
            TimeSpan rawDiff;
            if (end >= begin)
            {
                rawDiff = end - begin;
            }
            else
            {
                rawDiff = (DateTime.Today.AddDays(1) - begin) + end.TimeOfDay;
            }
            return rawDiff;
        }

        public DateTime? Begin { get; set; }

        public DateTime? End { get; set; }

        public string Comment { get; set; }
    }
}

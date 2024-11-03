using System;
using System.Globalization;

namespace Plainion.WhiteRabbit.Presentation.Reports
{
    public class ReportBase
    {
        public bool IsComplete { get; set; }

        protected static string FormatTimeSpan(TimeSpan span)
        {
            if (span == TimeSpan.Zero)
            {
                return "-";
            }

            return string.Format(CultureInfo.InvariantCulture, "{0,2:00}:{1,2:00}", span.Days * 24 + span.Hours, span.Minutes);
        }
    }
}
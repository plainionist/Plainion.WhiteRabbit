using System;
using System.Collections.Generic;
using System.IO;

namespace Plainion.WhiteRabbit.Reports
{
    public partial class DayReport : ReportBase
    {
        public DateTime Day { get; set; }
        public Dictionary<string, TimeSpan> Data { get; set; }

        public void Generate(TextWriter writer)
        {
            writer.WriteLine($"<html><head><title>WhiteRabbit</title></head>");
            writer.WriteLine($"<body>");
            writer.WriteLine($"  <center><h2>{Day.Date.ToShortDateString()}</h2></center>");

            writer.WriteLine($"  <table border='0' cellpadding='4' cellspacing='0'>");
            writer.WriteLine($"    <tr>");
            writer.WriteLine($"      <th>Comment</th>");
            writer.WriteLine($"      <th style='width:75px;' align='right'>Time</th>");
            writer.WriteLine($"    </tr>");


            var sum = new TimeSpan();
            foreach (string comment in Data.Keys)
            {
                if (comment == "unknown")
                {
                    continue;
                }

                sum += Data[comment];

                writer.WriteLine($"    <tr>");
                writer.WriteLine($"      <td>{comment}</td>");
                writer.WriteLine($"      <td align='right'>{FormatTimeSpan(Data[comment])}</td>");
                writer.WriteLine($"    </tr>");
            }

            if (Data["unknown"] != TimeSpan.Zero)
            {
                sum += Data["unknown"];

                writer.WriteLine($"    <tr>");
                writer.WriteLine($"      <td>Unknown</td>");
                writer.WriteLine($"      <td align='right'>{FormatTimeSpan(Data["unknown"])}</td>");
                writer.WriteLine($"    </tr>");
            }

            writer.WriteLine($"    <tr>");
            writer.WriteLine($"      <td style='border-top:solid 2px #060'><b>Sum</b></td>");
            writer.WriteLine($"      <td align='right' style='border-top:solid 2px #060'>{FormatTimeSpan(sum)}</td>");
            writer.WriteLine($"    </tr>");
            writer.WriteLine($"  </table>");

            if (!IsComplete)
            {
                writer.WriteLine($"  <font color='red'>The report is not complete because the data of the day is not complete.</font>");
            }

            writer.WriteLine($"</body></html>");
        }
    }
}

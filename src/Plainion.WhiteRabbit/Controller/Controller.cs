﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Plainion.WhiteRabbit.Model;
using Plainion.WhiteRabbit.Properties;
using Plainion.WhiteRabbit.Reports;
using Plainion.WhiteRabbit.View;

namespace Plainion.WhiteRabbit.Presentation
{
    public class Controller
    {
        private Recorder myRecorder;

        public Controller()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.DBStore))
            {
                Settings.Default.DBStore = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WhiteRabbit");
                Settings.Default.Save();
            }

            Database = new Database(Settings.Default.DBStore);

            MainView = ViewFactory.CreateMainView(this);
        }

        public Database Database { get; }
        public IView MainView { get; }
        public IView TimerView { get; private set; }
        public DataTable CurrentDayData { get; private set; }
        public DateTime CurrentDay { get; private set; }

        public void ChangeDay(DateTime day)
        {
            CurrentDay = day;
            CurrentDayData = GetTableByDay(day);
        }

        public void DeleteDayEntry(int idx)
        {
            CurrentDayData.Rows.RemoveAt(idx);
            CurrentDayData.AcceptChanges();

            Database.StoreTable(CurrentDayData);
        }

        private DataTable GetTableByDay(DateTime day)
        {
            var table = Database.LoadTable(day);
            if (table == null)
            {
                table = Database.CreateTable(day);
            }

            return table;
        }

        public void StartTimeMeasurement()
        {
            DayEntry entry = null;
            if (CurrentDayData.Rows.Count > 0)
            {
                var dr = CurrentDayData.Rows[CurrentDayData.Rows.Count - 1];

                entry = DayEntry.Parse(dr);
            }
            else
            {
                entry = new DayEntry();
            }

            MainView.Hide();

            if (TimerView == null)
            {
                TimerView = ViewFactory.CreateTimerView(this);
            }

            TimerView.Show();
            (TimerView as SlimForm).Start(entry);

            myRecorder = new Recorder(TimerView.Channel);

            if (entry.End == null)
            {
                if (null == entry.Begin)
                {
                    myRecorder.Start();
                }
                else
                {
                    myRecorder.Start(entry.Begin.Value);
                }
            }
            else
            {
                myRecorder.Start();
            }
        }

        public void StopTimeMeasurement(string comment)
        {
            TimerView.Hide();
            MainView.Show();

            myRecorder.Stop();

            // if last record is not yet completed
            // -> complete this one
            DataRow dr = null;
            if (CurrentDayData.Rows.Count > 0)
            {
                dr = CurrentDayData.Rows[CurrentDayData.Rows.Count - 1];
            }

            if (dr != null && dr.IsEmpty(ColumnNames.END))
            {
                dr[ColumnNames.END] = myRecorder.StopTime.ToShortTimeString();

                dr[ColumnNames.COMMENT] = comment;

                if (dr.IsEmpty(ColumnNames.BEGIN))
                {
                    dr[ColumnNames.BEGIN] = myRecorder.StartTime.ToShortTimeString();
                }
            }
            else
            {
                // add recorded data to table
                dr = CurrentDayData.NewRow();
                CurrentDayData.Rows.Add(dr);
                CurrentDayData.AcceptChanges();

                dr[ColumnNames.BEGIN] = myRecorder.StartTime.ToShortTimeString();
                dr[ColumnNames.END] = myRecorder.StopTime.ToShortTimeString();
                dr[ColumnNames.COMMENT] = comment;
            }

            Database.StoreTable(CurrentDayData);

            myRecorder = null;
        }

        public string GenerateDayReport(DateTime day)
        {
            var file = Path.Combine(Path.GetTempPath(), "WhiteRabbit.Report.html");

            var data = GetDetails(day, out bool isComplete);

            using (var writer = new StreamWriter(file))
            {
                var report = new DayReport
                {
                    Day = day,
                    Data = data,
                    IsComplete = isComplete
                };
                report.Generate(writer);
            }

            return file;
        }

        public string GenerateWeekReport(DateTime day)
        {
            var begin = GetBeginOfWeek(day);
            var end = GetEndOfWeek(day);
            return GenerateRangeReport(begin, end);
        }

        private DateTime GetBeginOfWeek(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        private DateTime GetEndOfWeek(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public string GenerateMonthReport(DateTime day)
        {
            var begin = new DateTime(day.Year, day.Month, 1);
            var end = new DateTime(day.Year, day.Month, DateTime.DaysInMonth(day.Year, day.Month));
            return GenerateRangeReport(begin, end);
        }

        public string GenerateRangeReport(DateTime begin, DateTime end)
        {
            var file = Path.Combine(Path.GetTempPath(), "WhiteRabbit.Report.html");

            var overview = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
            {
                ["unknown"] = new TimeSpan()
            };

            var details = new Dictionary<DateTime, Dictionary<string, TimeSpan>>();

            bool isAllComplete = true;

            for (var date = begin; date <= end; date = date.AddDays(1))
            {
                bool isComplete;
                details[date] = GetDetails(date, out isComplete);

                // handle overview
                if (!isComplete)
                {
                    isAllComplete = false;
                }

                foreach (var entry in details[date])
                {
                    AddTimeSpan(overview, entry.Key, entry.Value);
                }
            }

            using (var writer = new StreamWriter(file))
            {
                var report = new RangeReport
                {
                    Begin = begin,
                    End = end,
                    Overview = overview,
                    Details = details,
                    IsComplete = isAllComplete
                };
                report.Generate(writer);
            }

            return file;
        }

        private Dictionary<string, TimeSpan> GetDetails(DateTime day, out bool isComplete)
        {
            var data = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
            {
                ["unknown"] = new TimeSpan()
            };

            isComplete = true;

            var table = Database.LoadTable(day);
            foreach (DataRow row in table.Rows)
            {
                var entry = DayEntry.Parse(row);
                var usedTime = entry.GetUsedTime();
                if (usedTime == null)
                {
                    isComplete = false;
                    continue;
                }

                var comment = entry.Comment ?? "unknown";

                AddTimeSpan(data, comment, usedTime.Value);
            }

            return data;
        }

        private void AddTimeSpan(Dictionary<string, TimeSpan> data, string comment, TimeSpan time)
        {
            if (!data.ContainsKey(comment))
            {
                data[comment] = time;
            }
            else
            {
                data[comment] += time;
            }
        }
    }
}
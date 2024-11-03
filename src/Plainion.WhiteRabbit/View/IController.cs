using System;
using System.Data;

namespace Plainion.WhiteRabbit.View
{
    internal interface IController
    {
        DataTable CurrentDayData { get; }
        DateTime CurrentDay { get; }
        void ChangeDay(DateTime day);
        void DeleteDayEntry(int idx);

        void TableCellChanged();

        void StartTimeMeasurement();
        void StopTimeMeasurement(string comment);

        string GenerateDayReport(DateTime day);
        string GenerateWeekReport(DateTime day);
        string GenerateMonthReport(DateTime day);
    }
}

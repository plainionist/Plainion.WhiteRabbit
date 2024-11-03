using System;
using System.Data;

namespace Plainion.WhiteRabbit.Model
{
    public static class DataRowExtensions
    {
        public static bool IsEmpty(this DataRow row, string column)
        {
            var value = row[column];

            if (value == null || value == DBNull.Value)
            {
                return true;
            }

            string s = value as string;
            if (s != null)
            {
                return string.IsNullOrWhiteSpace(s);
            }

            return false;
        }
    }
}
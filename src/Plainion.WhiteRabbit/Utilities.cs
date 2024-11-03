using System;

namespace Plainion.WhiteRabbit
{
    public static class ObjectExtensions
    {
        public static bool IsEmpty(this object value)
        {
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
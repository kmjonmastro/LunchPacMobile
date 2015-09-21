using System;
using System.Xml;

namespace LunchPac
{
    public static class Extensions
    {
        public static string ToISOString(this DateTime time)
        {
            return time.ToString("O");
        }
    }
}


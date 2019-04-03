using System;
using System.Collections.Generic;

namespace App
{
    public static class StringExtenstions
    {
        public static IEnumerable<TimeSpan?> ToTimeSpans(this IEnumerable<string> source)
        {
            foreach (var v in source)
            {
                if (string.IsNullOrEmpty(v))
                {
                    yield return null;
                }
                else
                {
                    yield return TimeSpan.Parse(v);
                }
            }
        }
    }
}
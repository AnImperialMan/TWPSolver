using System;
using System.Collections.Generic;
using System.Linq;

namespace TWPPract
{
    public static class Extensions
    {
        private static readonly Dictionary<int, string> LowerNumbers = new Dictionary<int, string>()
        {
            {0, "₀"},
            {1, "₁"},
            {2, "₂"},
            {3, "₃"},
            {4, "₄"},
            {5, "₅"},
            {6, "₆"},
            {7, "₇"},
            {8, "₈"},
            {9, "₉"},
        };

        private static IEnumerable<int> GetDigits(int source)
        {
            int individualFactor = 0;
            int tennerFactor = Convert.ToInt32(Math.Pow(10, source.ToString().Length));
            do
            {
                source -= tennerFactor * individualFactor;
                tennerFactor /= 10;
                individualFactor = source / tennerFactor;

                yield return individualFactor;
            } while (tennerFactor > 1);
        }
        
        public static string ToLowerUnicode(this byte item)
        {
            var digits = GetDigits(item).ToArray();
            return digits.Aggregate("", (current, digit) => current + LowerNumbers[digit]);
        }
        
        public static string ToLowerUnicode(this int item)
        {
            return ToLowerUnicode((byte) item);
        }

        public static string Join(this string[] links)
        {
            var retStr = "";
            char prevLink = '\0';
            foreach (var link in links)
            {
                if (link[0] == prevLink)
                {
                    retStr += link.Substring(1);
                }
                else
                {
                    retStr += link;
                }
                prevLink = link[0];
            }

            return retStr;
        }
    }
}
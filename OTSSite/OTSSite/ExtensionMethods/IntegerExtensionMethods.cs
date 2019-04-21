using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ExtensionMethods
{
    public static class IntegerExtensionMethods
    { 
        private enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public static string ToMonthName(this int month)
        {
            return ((Month)month).ToString();
        }
    }
}

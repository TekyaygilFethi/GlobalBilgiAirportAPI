using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Helpers
{
    public static class StringExtensions
    {
        public static bool CheckIsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Cowin.Infrastructure.Dates
{
    public static class DateHelpers
    {
        public static bool ValidDateForApis(string date)
        {
            var parts = date.Split("-", StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length != 3) return false;
            
            if (!int.TryParse(parts[0], out var day)) return false;
            if (day < 1 || day > 31) return false;

            if (!int.TryParse(parts[1], out var month)) return false;
            if (month < 1 || month > 12) return false;

            if (!int.TryParse(parts[2], out var year)) return false;
            if (year < 2021 || year > 2022) return false;

            return true;
        }

        public static List<string> GetStartDatesForUpcomingWeeks(string start, int countNextWeeks)
        {
            if (!string.IsNullOrEmpty(start) && !ValidDateForApis(start)) return null;
            if (string.IsNullOrEmpty(start)) start = DateTime.Now.ToString("dd-MM-yyyy");

            var startDate = DateTime.ParseExact(start, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var week1Start = startDate.GetMonday();

            var result = new List<string>();
            result.Add(week1Start.ToString("dd-MM-yyyy"));
            for (int i = 1; i <= countNextWeeks; ++i) {
                var next = week1Start.AddDays(7 * i).Date;
                result.Add(next.ToString("dd-MM-yyyy"));
            }

            return result;
        }

        public static DateTime GetMonday(this DateTime dateTime)
        {
            int aheadBy = (7 + (dateTime.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dateTime.AddDays(-1 * aheadBy).Date;
        }
    }
}

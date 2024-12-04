    using System.Text;
    public static class TimeStringExtensions
    {
        public static string RemainingString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return $"{(int)timeSpan.TotalDays} dagar {timeSpan.Hours} timmar";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                return $"{timeSpan.Hours} timmar {timeSpan.Minutes} minuter";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                return $"{timeSpan.Minutes} minuter";
            }
            else
            {
                return "Mindre än en minut";
            }
        }

        public static string WordDateString(this DateTime dateTime)
        {
            Dictionary<int, string> months = new()
            {
                [1] = "januari",
                [2] = "februari",
                [3] = "mars",
                [4] = "april",
                [5] = "maj",
                [6] = "juni",
                [7] = "juli",
                [8] = "augusti",
                [9] = "september",
                [10] = "oktober",
                [11] = "november",
                [12] = "december"
            };
            return $"{months[dateTime.Month]} {dateTime.Year}";
        }
    }
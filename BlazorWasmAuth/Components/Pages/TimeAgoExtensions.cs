using Frontend.ResourceFiles;
using Microsoft.Extensions.Localization;

namespace Frontend.Components.Pages
{
    internal static class TimeAgoExtensions
    {
        public static string ToTimeAgo(this DateTime dateTime)
        {
            var ts = DateTime.UtcNow - dateTime.ToUniversalTime();

            if (ts.TotalSeconds < 60)
                return $"{ts.Seconds} seconds ago";

            if (ts.TotalMinutes < 60)
                return $"{ts.Minutes} minutes ago";

            if (ts.TotalHours < 24)
                return $"{ts.Hours} hours ago";

            if (ts.TotalDays < 30)
                return $"{ts.Days} days ago";

            if (ts.TotalDays < 365)
            {
                int months = (int)(ts.TotalDays / 30);
                return months == 1 ? "1 month ago" : $"{months} months ago";
            }

            int years = (int)(ts.TotalDays / 365);
            return years == 1 ? "1 year ago" : $"{years} years ago";
        }

        public static string ToTimeAgo(this DateTime dateTime, IStringLocalizer<Resource> localizer)
        {
            var ts = DateTime.UtcNow - dateTime.ToUniversalTime();

            if (ts.TotalSeconds < 60)
                return localizer["SecondsAgo", ts.Seconds];

            if (ts.TotalMinutes < 60)
                return localizer["MinutesAgo", ts.Minutes];

            if (ts.TotalHours < 24)
                return localizer["HoursAgo", ts.Hours];

            if (ts.TotalDays < 30)
                return localizer["DaysAgo", ts.Days];

            if (ts.TotalDays < 365)
            {
                int months = (int)(ts.TotalDays / 30);
                return months == 1
                    ? localizer["OneMonthAgo"]
                    : localizer["ManyMonthsAgo", months];
            }

            int years = (int)(ts.TotalDays / 365);
            return years == 1
                ? localizer["OneYearAgo"]
                : localizer["ManyYearsAgo", years];
        }

    }
}

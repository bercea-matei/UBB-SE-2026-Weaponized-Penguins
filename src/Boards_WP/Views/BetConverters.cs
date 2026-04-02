using Microsoft.UI.Xaml.Data;

using System;

namespace Boards_WP.Views
{

    // we need IValueConvertor because the Bet model contains the raw data,
    // that does not match the specific string formats required by the UI design
    public class OddsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "1.85x";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }

    // turning the remaining time as a string, instead of datetime
    public class TimeRemainingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime endingTime)
            {
                TimeSpan remaining = endingTime - DateTime.Now;

                if (remaining.Ticks <= 0) return "Ended";
                return $"{(int)remaining.TotalHours:D2}h {remaining.Minutes:D2}m";
            }
            return "00h 00m";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isVisible)
            {
                return isVisible ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            return Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
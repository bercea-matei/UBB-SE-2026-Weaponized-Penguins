using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Boards_WP.Data.Models;

using Windows.ApplicationModel.Background;

namespace Boards_WP.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private Brush _appThemeBrush;

        private readonly Windows.UI.Color _defaultColor = Windows.UI.Color.FromArgb(255, 230, 226, 255); // #E6E2FF

        public MainViewModel()
        {
            AppThemeBrush = new SolidColorBrush(_defaultColor);
        }

        public void ApplyNewTheme(ThemeColor newDominantColor)
        {
            if (newDominantColor == ThemeColor.Default)
            {
                AppThemeBrush = new SolidColorBrush(_defaultColor);
                return;
            }

            Windows.UI.Color actualColor = MapEnumToUiColor(newDominantColor);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Windows.Foundation.Point(0, 0),
                EndPoint = new Windows.Foundation.Point(1, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop { Color = actualColor, Offset = 0.0 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = actualColor, Offset = 0.7 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = _defaultColor, Offset = 1.0 });

            AppThemeBrush = gradientBrush;
        }

        private Windows.UI.Color MapEnumToUiColor(ThemeColor colorEnum)
        {
            return colorEnum switch
            {
                ThemeColor.Pink => Windows.UI.Color.FromArgb(255, 255, 192, 203),
                ThemeColor.Blue => Windows.UI.Color.FromArgb(255, 173, 216, 230),
                ThemeColor.Green => Windows.UI.Color.FromArgb(255, 144, 238, 144),
                ThemeColor.Orange => Windows.UI.Color.FromArgb(255, 255, 165, 0),
                ThemeColor.Turquoise => Windows.UI.Color.FromArgb(255, 64, 224, 208),
                ThemeColor.Yellow => Windows.UI.Color.FromArgb(255, 255, 255, 0),
                ThemeColor.Red => Windows.UI.Color.FromArgb(255, 255, 0, 0),
                ThemeColor.Purple => Windows.UI.Color.FromArgb(255, 128, 0, 128),
                ThemeColor.Default => _defaultColor,
                _ => _defaultColor
            };
        }
    }
}
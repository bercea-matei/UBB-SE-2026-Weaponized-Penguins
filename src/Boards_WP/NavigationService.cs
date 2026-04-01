using System;

using Microsoft.UI.Xaml.Controls; // Keep this here

using Boards_WP.Data.Services.Interfaces;

namespace Boards_WP.Data.Services;

public class NavigationService : INavigationService
{
    private Frame? _frame;

    public bool CanGoBack => _frame?.CanGoBack ?? false;

    // Accept an 'object' to satisfy the interface
    public void Initialize(object frame)
    {
        // Safely cast it back to a WinUI Frame
        _frame = frame as Frame;
    }

    public void NavigateTo(Type pageType, object? parameter = null)
    {
        if (_frame != null && _frame.CurrentSourcePageType != pageType)
        {
            _frame.Navigate(pageType, parameter);
        }
    }

    public void GoBack()
    {
        if (CanGoBack)
        {
            _frame?.GoBack();
        }
    }
}
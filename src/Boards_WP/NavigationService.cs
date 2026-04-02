using System;
using Microsoft.UI.Xaml.Controls; 
using Boards_WP;
namespace Boards_WP;

public class NavigationService : INavigationService
{
    private Frame? _frame;

    public bool CanGoBack => _frame?.CanGoBack ?? false;

    public void Initialize(object frame)
    {
        _frame = frame as Frame;
    }

    public void NavigateTo(Type pageType, object? parameter = null)
    {
        if (_frame == null)
        {
            System.Diagnostics.Debug.WriteLine("NavigationService Error: _frame is null. Did you call Initialize()?");
            return;
        }
        if (parameter != null)
        {
            _frame.Navigate(pageType, parameter);
        }
        else
        {
            _frame.Navigate(pageType);
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
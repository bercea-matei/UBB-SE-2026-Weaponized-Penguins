using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using Boards_WP.ViewModels;

namespace Boards_WP.Views.Pages
{
    public sealed partial class LoginView : Page
    {
        // Property for XAML Binding
        public LoginViewModel ViewModel { get; }

        public LoginView()
        {
            this.InitializeComponent();

            // Resolve the ViewModel from the service provider
            ViewModel = App.Services.GetRequiredService<LoginViewModel>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

using Boards_WP.Data.Services.Interfaces;
using Boards_WP.Data.Services;
using Boards_WP.ViewModels;

namespace Boards_WP;

public partial class App : Application
{
    public Window? m_window;

    public static IServiceProvider Services { get; private set; } = null!;

    public static T GetService<T>() where T : class
        => Services.GetRequiredService<T>();

    public App()
    {
        Services = ConfigureServices();
        InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ICommunitiesService, CommunitiesService>();

        // ViewModels
        services.AddTransient<CreateCommunityViewModel>();

        return services.BuildServiceProvider();
    }
}
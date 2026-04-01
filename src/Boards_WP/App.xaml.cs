using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Boards_WP.Data.Services;
using Boards_WP.Data.Services.Interfaces;
using Boards_WP.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Boards_WP;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public Window? m_window;

    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        try
        {
            InitializeComponent();
            Services = ConfigureServices();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Communities;Trusted_Connection=True;TrustServerCertificate=True;";
        services.AddSingleton(connectionString);

        services.AddSingleton<IBetsRepository, BetsRepository>();
        services.AddSingleton<ICommentsRepository, CommentsRepository>();
        services.AddSingleton<ICommunitiesRepository, CommunitiesRepository>();
        services.AddSingleton<INotificationRepository, NotificationRepository>();
        services.AddSingleton<IPostsRepository, PostsRepository>();
        services.AddSingleton<ITagsRepository, TagsRepository>();
        services.AddSingleton<IUsersMoodRepository, UsersMoodRepository>();
        services.AddSingleton<IUsersRepository, UsersRepository>();

        services.AddSingleton<IBetsService, BetsService>();
        services.AddSingleton<ICommentsService, CommentsService>();
        services.AddSingleton<ICommunitiesService, CommunitiesService>();
        services.AddSingleton<INotificationsService, NotificationsService>();
        services.AddSingleton<IPostsService, PostsService>();
        services.AddSingleton<IUsersService, UsersService>();

        services.AddSingleton<UserSession>();

        services.AddTransient<HeaderViewModel>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }
}

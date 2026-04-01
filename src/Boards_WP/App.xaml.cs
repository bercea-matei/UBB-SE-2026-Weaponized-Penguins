using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Boards_WP.Data.Services;
using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;

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

public partial class App : Application
{
    public Window? m_window;

    public new static App Current => (App)Application.Current;
    public IServiceProvider? Services { get; }

    public App()
    {
        InitializeComponent();

        Services = new ServiceCollection()

            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<UserSession>()

            .AddSingleton<IPostsService, PostsService>()
            .AddSingleton<ICommunitiesService, CommunitiesService>()
            .AddSingleton<INotificationsService, NotificationsService>()
            .AddSingleton<ICommentsService, CommentsService>()
            .AddSingleton<IBetsService, BetsService>()
            .AddSingleton<IUsersService, UsersService>()

            .AddSingleton<string>("Data Source=DESKTOP\\SQLEXPRESS;Initial Catalog=Communities;Integrated Security=True;Encrypt=False;TrustServerCertificate=True") 
            .AddSingleton<IPostsRepository, PostsRepository>()
            .AddSingleton<ICommunitiesRepository, CommunitiesRepository>()
            .AddSingleton<INotificationRepository, NotificationRepository>()
            .AddSingleton<ICommentsRepository, CommentsRepository>()
            .AddSingleton<IBetsRepository, BetsRepository>()
            .AddSingleton<IUsersRepository, UsersRepository>()
            .AddSingleton<ITagsRepository, TagsRepository>()
            .AddSingleton<IUsersMoodRepository, UsersMoodRepository>()

            .AddSingleton<FeedViewModel>()
            //VMs
            //.AddTransient<CommunityViewModel>()
            //.AddTransient<CreateCommunityViewModel>()
            //.AddTransient<CreatePostViewModel>()
            .AddTransient<FullPostViewModel>()
            //.AddTransient<NotificationsViewModel>()
            .AddTransient<MainViewModel>()

            // Components / UserControls
            //.AddTransient<PostPreviewViewModel>();
            //.AddTransient<CommentViewModel>();
            .AddTransient<CommunityBarViewModel>()
            //.AddTransient<HeaderViewModel>();

            .BuildServiceProvider();
    }

    
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();

        var navService = Services.GetRequiredService<INavigationService>() as NavigationService;
        if (m_window.Content is FrameworkElement root)
        {
            var frame = root.FindName("ContentFrame") as Frame;
            if (frame != null)
            {
                navService.Initialize(frame);
                navService.NavigateTo(typeof(FeedView));
            }
        }

        m_window.Activate();
    }
}

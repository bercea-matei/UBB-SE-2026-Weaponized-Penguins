using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Boards_WP.Data.Repositories;
using Boards_WP.Data.Repositories.Interfaces;
using Boards_WP.Data.Services;
using Boards_WP.Data.Services.Interfaces;
using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;

namespace Boards_WP;

public partial class App : Application
{
    public Window? m_window;
    public new static App Current => (App)Application.Current;
    public static IServiceProvider Services { get; private set; } = null!;
    public static T GetService<T>() where T : class
        => Services.GetRequiredService<T>();

    public App()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<string>("Data Source=DESKTOP-GFA6UNJ\\SQLEXPRESS;Initial Catalog=WeaponizedPenguins;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");

        serviceCollection.AddSingleton<IPostsRepository, PostsRepository>();
        serviceCollection.AddSingleton<ICommunitiesRepository, CommunitiesRepository>();
        serviceCollection.AddSingleton<INotificationRepository, NotificationRepository>();
        serviceCollection.AddSingleton<ICommentsRepository, CommentsRepository>();
        serviceCollection.AddSingleton<IBetsRepository, BetsRepository>();
        serviceCollection.AddSingleton<IUsersRepository, UsersRepository>();
        serviceCollection.AddSingleton<ITagsRepository, TagsRepository>();
        serviceCollection.AddSingleton<IUsersMoodRepository, UsersMoodRepository>();

        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<IPostsService, PostsService>();
        serviceCollection.AddSingleton<ICommunitiesService, CommunitiesService>();
        serviceCollection.AddSingleton<INotificationsService, NotificationsService>();
        serviceCollection.AddSingleton<ICommentsService, CommentsService>();
        serviceCollection.AddSingleton<IBetsService, BetsService>();
        serviceCollection.AddSingleton<IUsersService, UsersService>();
        serviceCollection.AddSingleton<UserSession>();

        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<FeedViewModel>();
        serviceCollection.AddTransient<CommunityBarViewModel>();
        serviceCollection.AddTransient<CreateCommunityViewModel>();
        serviceCollection.AddTransient<UpdateCommunityViewModel>();
        serviceCollection.AddTransient<CommunityViewModel>();
        serviceCollection.AddTransient<FullPostViewModel>();
        serviceCollection.AddTransient<NotificationsListViewModel>();
        serviceCollection.AddTransient<PostPreviewViewModel>();
        serviceCollection.AddTransient<CommentViewModel>();
        serviceCollection.AddTransient<NotificationItemViewModel>();
        serviceCollection.AddTransient<CreateTagViewModel>();
        //serviceCollection.AddTransient<CreatePostViewModel>();

        
        Services = serviceCollection.BuildServiceProvider();
        this.InitializeComponent();
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
                navService?.Initialize(frame);
                navService?.NavigateTo(typeof(FeedView));
            }
        }

        m_window.Activate();
    }
}
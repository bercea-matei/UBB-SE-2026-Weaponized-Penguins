using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Boards_WP.ViewModels;

namespace Boards_WP.Views
{
    public sealed partial class NotificationsView : UserControl
    {
        public NotificationsListViewModel ViewModel { get; } = new NotificationsListViewModel();

        public NotificationsView()
        {
            this.InitializeComponent();
            NotificationsList.ItemsSource = ViewModel.Notifications;
        }

        private void NotificationsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NotificationItemViewModel itemViewModel)
            {
                itemViewModel.OpenNotificationCommand.Execute(null);
            }
        }
    }
}
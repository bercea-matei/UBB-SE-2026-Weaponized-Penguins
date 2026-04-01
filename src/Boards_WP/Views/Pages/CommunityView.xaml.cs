using System;
using System.Collections.ObjectModel;
using System.IO;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

using Boards_WP.Data.Models;

namespace Boards_WP.Views.Pages
{
    public sealed partial class CommunityView : Page
    {

        // allowing other pages to pass a Community object into this page
        public static readonly DependencyProperty CurrentCommunityProperty =
            DependencyProperty.Register("CurrentCommunity", typeof(Community), typeof(CommunityView), new PropertyMetadata(null));

        public Community CurrentCommunity
        {
            get => (Community)GetValue(CurrentCommunityProperty);
            set => SetValue(CurrentCommunityProperty, value);
        }

        public static readonly DependencyProperty IsMemberProperty =
            DependencyProperty.Register("IsMember", typeof(bool), typeof(CommunityView), new PropertyMetadata(false));

        public bool IsMember
        {
            get => (bool)GetValue(IsMemberProperty);
            set => SetValue(IsMemberProperty, value);
        }

        public ObservableCollection<Post> CommunityPosts { get; set; } = new();

        public CommunityView()
        {
            this.InitializeComponent();
        }

        public BitmapImage BannerImage => ConvertToBitmap(CurrentCommunity?.Banner);
        public BitmapImage ProfileImage => ConvertToBitmap(CurrentCommunity?.Picture);
        public string MemberCountText => $"{CurrentCommunity?.MembersNumber ?? 0} members";
        public Visibility JoinButtonVisibility => IsMember ? Visibility.Collapsed : Visibility.Visible;
        public Visibility MemberActionsVisibility => IsMember ? Visibility.Visible : Visibility.Collapsed;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Community community)
            {
                CurrentCommunity = community;
                LoadDefaultPosts();
                IsMember = (community.Admin?.Username == "@Me");
            }
            else if (e.Parameter is Post newPost)  // when a new post was created and needs to be displayed
            {
                CurrentCommunity = newPost.ParentCommunity;
                IsMember = true;
                CommunityPosts.Add(newPost);
            }
            this.Bindings.Update();
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            IsMember = true;
            if (CurrentCommunity != null) CurrentCommunity.MembersNumber++;
            this.Bindings.Update();
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            IsMember = false;
            if (CurrentCommunity != null) CurrentCommunity.MembersNumber--;
            this.Bindings.Update();
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreatePostView), CurrentCommunity);
        }

     
        private BitmapImage ConvertToBitmap(byte[] data)
        {
            if (data == null || data.Length == 0) return null;
            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(data))
            {
                var stream = ms.AsRandomAccessStream();
                bitmap.SetSource(stream);
            }
            return bitmap;
        }

        private void LoadDefaultPosts()
        {
            CommunityPosts.Clear();
            CommunityPosts.Add(new Post { Title = "Welcome!", Owner = new User { Username = "@Admin" }, Score = 50, Description = "Welcome to the community!" });
        }
    }
}
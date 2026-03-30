using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;


namespace Boards_WP.Views.Pages
{
    public sealed partial class CommunityView : Page
    {
        public Community CurrentCommunity { get; set; } // the one the user is looking at
        public ObservableCollection<Post> CommunityPosts { get; set; } = new ObservableCollection<Post>();

        // properties for XAML binding
        public BitmapImage BannerImage => ConvertToBitmap(CurrentCommunity?.Banner);
        public BitmapImage ProfileImage => ConvertToBitmap(CurrentCommunity?.Picture);
        public string MemberCountText => $"{CurrentCommunity?.MembersNumber} members";

        public CommunityView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Community community)
            {
                CurrentCommunity = community;
                Bindings.Update();
                LoadCommunityPosts(community.CommunityID);
            }
        }

        // this function converts byte[] (from the images in the database) into BitmapImage, because XAML doesn't know byte[]
        private BitmapImage ConvertToBitmap(byte[] data)
        {
            if (data == null || data.Length == 0) return null;

            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(data)) // turning the image into a stream
            {
                var randomAccessStream = ms.AsRandomAccessStream();
                bitmap.SetSource(randomAccessStream);
            }
            return bitmap;
        }

       
        private void LoadCommunityPosts(int id)
        {
            CommunityPosts.Clear();
            CommunityPosts.Add(new Post
            {
                PostID = 1,
                Title = "Welcome!",
                Owner = new User { Username = "@AlexBindiu" },
                Community = new Community { Name = "Computer Science" },
                Score = 50,
                CommentsNumber = 30,
                CreationTime = new DateTime(2026, 3, 30),
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            });
            CommunityPosts.Add(new Post
            {
                PostID = 1,
                Title = "Community Rules",
                Owner = new User { Username = "@Alexandra" },
                Community = new Community { Name = "Computer Science" },
                Score = 34,
                CommentsNumber = 10,
                CreationTime = new DateTime(2026, 3, 30),
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            });
        }

        // this function opens the full view of a post in preview form inside a community
        // sender: is the PostPreviewForm the user clicked on
        // e: contains info about the click (not very relevant)

        private void ClickOnPostFromCommunity(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // converting the sender in a FrameworkElement, so we can see exactly which post in full view do we need to open (using the DataContext)
            // will call this post "selectedPost"
            if (sender is FrameworkElement fe && fe.DataContext is Post selectedPost)
            {
                // going to the FullPostView page of the selected post
                this.Frame.Navigate(typeof(FullPostView), selectedPost);
            }
        }
    }
}
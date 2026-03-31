using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;


namespace Boards_WP.Views.Pages
{   

    // we will use INotifyPropertyChanged to tell the interface to change whenever a variable changes in C#
    // the actual function which "shouts" the notification is at the bottom
    public sealed partial class CommunityView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Community _currentCommunity; // initially null
        public Community CurrentCommunity { // the one the user is looking at
            get => _currentCommunity;
            set { _currentCommunity = value; OnPropertyChanged(); } // the UI will display the current community
        }


        public ObservableCollection<Post> CommunityPosts { get; set; } = new();


        private bool _isMember = false; // initially false
        
        // when _isMember becomes true (so when the Join button gets clicked), the "set" code will run from the method IsMember
        public bool IsMember
        {
            get => _isMember;
            set  
            {
                _isMember = value;
                OnPropertyChanged();
                this.Bindings.Update(); // updates the button visibility
            }
        }

        // properties for XAML binding
        public BitmapImage BannerImage => ConvertToBitmap(CurrentCommunity?.Banner);
        public BitmapImage ProfileImage => ConvertToBitmap(CurrentCommunity?.Picture);
        public string MemberCountText => $"{CurrentCommunity?.MembersNumber ?? 0} members";

        public Visibility BoolToVisibility(bool isOpen) => isOpen ? Visibility.Visible : Visibility.Collapsed;
        public Visibility InverseBoolToVisibility(bool isOpen) => isOpen ? Visibility.Collapsed : Visibility.Visible;

        public CommunityView()
        {
            this.InitializeComponent();
        }


        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CommunityPosts.Clear();

            // 1) navigating from sidebar (parameter is a Community)
            if (e.Parameter is Community community)
            {
                CurrentCommunity = community;
                IsMember = false;
                 
            }
            // 2) returning from CreatePostView (parameter is the new Post)
            else if (e.Parameter is Post newPost)
            {
                CurrentCommunity = newPost.ParentCommunity;
                IsMember = true; // User must be a member to have created a post
                CommunityPosts.Add(newPost);
            }
            LoadDefaultPosts();
            this.Bindings.Update();
        }

        private void LoadDefaultPosts()
        {
            CommunityPosts.Add(new Post
            {
                PostID = 1,
                Title = "Welcome!",
                Owner = new User { Username = "@AlexBindiu" },
                ParentCommunity = new Community { Name = "Computer Science", Admin = new User { Username = "System" } },
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
                ParentCommunity = new Community { Name = "Computer Science", Admin = new User { Username = "System" } },
                Score = 34,
                CommentsNumber = 10,
                CreationTime = new DateTime(2026, 3, 30),
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            });
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            // passing the CurrentCommunity so the form knows where it belongs
            this.Frame.Navigate(typeof(CreatePostView), CurrentCommunity);
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            IsMember = true;
            if (CurrentCommunity != null)
            {
                CurrentCommunity.MembersNumber++;
                this.Bindings.Update(); 
            }
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            IsMember = false;
            if (CurrentCommunity != null)
            {
                CurrentCommunity.MembersNumber--;
                this.Bindings.Update(); // refreshing the member count text
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
                ParentCommunity = new Community { Name = "Computer Science", Admin = new User { Username = "System" } },
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
                ParentCommunity = new Community { Name = "Computer Science", Admin = new User { Username = "System" } },
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
        
        // this function sends a notification to Windows UI, announcing that the IsMember variable has changed
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
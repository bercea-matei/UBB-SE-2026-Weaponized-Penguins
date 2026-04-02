using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Boards_WP.ViewModels
{
    public partial class CommunityViewModel : ObservableObject
    {
        private readonly IPostsService _postsService;
        private readonly ICommunitiesService _communitiesService;
        private readonly UserSession _userSession;
        private readonly Action<Community> _navigateToCreatePost;

        private static ObservableCollection<Community> _sidebarList;

   
        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(BannerImage),
            nameof(ProfileImage),
            nameof(MemberCountText))]
        private Community _currentCommunity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(JoinButtonVisibility),
            nameof(MemberActionsVisibility))]
        [NotifyCanExecuteChangedFor(
            nameof(JoinCommand),
            nameof(LeaveCommand),
            nameof(CreatePostCommand))]
        private bool _isMember;

        public ObservableCollection<PostPreviewViewModel> CommunityPosts { get; } = new();

        public BitmapImage BannerImage => ConvertToBitmap(CurrentCommunity?.Banner);
        public BitmapImage ProfileImage => ConvertToBitmap(CurrentCommunity?.Picture);
        public string MemberCountText => $"{CurrentCommunity?.MembersNumber ?? 0} members";

        public Visibility JoinButtonVisibility => IsMember ? Visibility.Collapsed : Visibility.Visible;
        public Visibility MemberActionsVisibility => IsMember ? Visibility.Visible : Visibility.Collapsed;

        public CommunityViewModel(Action<Community> navigateToCreatePost)
        {
            _postsService = App.Services?.GetService<IPostsService>();
            _communitiesService = App.Services?.GetService<ICommunitiesService>();
            _userSession = App.Services?.GetService<UserSession>();
            _navigateToCreatePost = navigateToCreatePost;
        }

        [RelayCommand(CanExecute = nameof(CanJoin))]
        private void Join()
        {
            var userId = _userSession.CurrentUser.UserID;
            _communitiesService.AddUser(CurrentCommunity.CommunityID, userId);

            IsMember = true;
            CurrentCommunity.MembersNumber++;
            OnPropertyChanged(nameof(MemberCountText));

            App.GetService<CommunityBarViewModel>().LoadCommunities();
        }
        private bool CanJoin() => !IsMember;

        [RelayCommand(CanExecute = nameof(CanLeave))]
        private void Leave()
        {
            var userId = _userSession.CurrentUser.UserID;

            if (_communitiesService.CheckOwner(CurrentCommunity.CommunityID, userId))
            {
                return;
            }

            _communitiesService.RemoveUser(CurrentCommunity.CommunityID, userId);

            IsMember = false;
            CurrentCommunity.MembersNumber--;
            OnPropertyChanged(nameof(MemberCountText));

            App.GetService<CommunityBarViewModel>().LoadCommunities();
        }
        private bool CanLeave() => IsMember;

        [RelayCommand(CanExecute = nameof(CanCreatePost))]
        private void CreatePost() => _navigateToCreatePost?.Invoke(CurrentCommunity);
        private bool CanCreatePost() => IsMember && CurrentCommunity != null;

        public void ApplyNavigationParameter(object parameter)
        {
            if (parameter is Community community)
            {
                CurrentCommunity = community;
                var userId = _userSession.CurrentUser?.UserID ?? 0;

                bool isActualMember = _communitiesService.IsPartOfCommunity(userId, community.CommunityID);
                bool isOwner = _communitiesService.CheckOwner(community.CommunityID, userId);

                IsMember = isActualMember || isOwner;

                CommunityPosts.Clear();
                LoadPosts(community.CommunityID);
            }
            
        }

        private void LoadPosts(int communityId)
        {
            CommunityPosts.Clear();
            var posts = _postsService.GetPostsByCommunityID(communityId);
            if (posts == null) return;
            var mainViewModel = App.Services?.GetService<MainViewModel>();
            foreach (var post in posts)
            {
                var previewVm = new PostPreviewViewModel(post, _postsService, _userSession, mainViewModel);
                CommunityPosts.Add(previewVm);
            }
        }

        private static BitmapImage ConvertToBitmap(byte[] data)
        {
            if (data == null || data.Length == 0) return null;
            var bitmap = new BitmapImage();
            using var ms = new MemoryStream(data);
            bitmap.SetSource(ms.AsRandomAccessStream());
            return bitmap;
        }
    }
}
using System;
using Boards_WP.Data.Models;
using Boards_WP.Data.Services.Interfaces;
using Boards_WP.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Boards_WP.ViewModels
{
    public partial class CreatePostViewModel : ObservableObject
    {
        private readonly IPostsService _postsService;
        private readonly INavigationService _navigationService;
        private readonly UserSession _userSession;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UploadPostCommand))]
        private string _postTitle = string.Empty;

        [ObservableProperty]
        private string _postDescription = string.Empty;

        [ObservableProperty]
        private string _tagsInput = string.Empty;

        public Community OriginCommunity { get; set; } = null!;

        public CreatePostViewModel(IPostsService postsService, INavigationService navigationService, UserSession userSession)
        {
            _postsService = postsService;
            _navigationService = navigationService;
            _userSession = userSession;
        }

        [RelayCommand(CanExecute = nameof(CanUploadPost))]
        private void UploadPost()
        {
            var newPost = new Post
            {
                Title = PostTitle,
                Description = PostDescription,
                ParentCommunity = OriginCommunity,
                Owner = _userSession.CurrentUser,
                Score = 0,
                CommentsNumber = 0,
                CreationTime = DateTime.Now
            };

            _postsService.AddPost(newPost);

            // Navigate back to the community screen just as original implemented intent.
            _navigationService.NavigateTo(typeof(CommunityView), OriginCommunity);
        }

        [RelayCommand]
        private void Cancel()
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }

        private bool CanUploadPost() => !string.IsNullOrWhiteSpace(PostTitle);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Boards_WP.Data.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

namespace Boards_WP.ViewModels
{
    public partial class FullPostViewModel : ObservableObject
    {
        [ObservableProperty]
        private Post _currentPost;

        [ObservableProperty]
        private string _newCommentText;


        public ObservableCollection<Comment> PostComments { get; } = new ObservableCollection<Comment>();

        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;
        private readonly MainViewModel _mainViewModel;
        private readonly UserSession _userSession;

        public FullPostViewModel()
        {
            _mainViewModel = App.Services?.GetService<MainViewModel>();
            _postsService = App.Services?.GetService<IPostsService>();
            _commentsService = App.Services?.GetService<ICommentsService>();
            _userSession = App.Services?.GetService<UserSession>();
        }

        public void LoadPost(Post post)
        {
            _currentPost = post;
            LoadRealComments();
        }

        private void LoadRealComments()
        {
            if (_currentPost == null) return;

            var comments = _commentsService.GetCommentsByPost(_currentPost.PostID, _userSession.CurrentUser.UserID);

            PostComments.Clear();
            foreach (var c in comments) 
            {
                PostComments.Add(c);
            }
        }

        [RelayCommand]
        private void Upvote()
        {
            if (CurrentPost == null) return;

            CurrentPost.Score++;
            OnPropertyChanged(nameof(CurrentPost));

            _postsService.IncreaseScore(CurrentPost.PostID);

            ThemeColor newThemeColor = _postsService.DetermineFeedThemeColorByLastLikes();

            _mainViewModel.ApplyNewTheme(newThemeColor);
        }

        [RelayCommand]
        private void Downvote()
        {
            if (CurrentPost == null) return;

            CurrentPost.Score--;
            OnPropertyChanged(nameof(CurrentPost));

            _postsService.DecreaseScore(CurrentPost.PostID);

            ThemeColor newThemeColor = _postsService.DetermineFeedThemeColorByLastLikes();
            _mainViewModel.ApplyNewTheme(newThemeColor);
        }

        [RelayCommand]
        private void PostComment()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText)) return;

            var newComment = new Comment
            {
                ParentPost = CurrentPost,
                Owner = _userSession.CurrentUser,
                Description = NewCommentText,
                Score = 0,
                Indentation = 0
            };

            try
            {
                _commentsService.AddComment(newComment);
                System.Diagnostics.Debug.WriteLine("Comment successfully added.");

                // Reload comments fully sorted rather than just inserting at top
                LoadRealComments();

                if (CurrentPost != null)
                {
                    CurrentPost.CommentsNumber++;
                    OnPropertyChanged(nameof(CurrentPost));
                }

                NewCommentText = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to add comment: {ex.Message} \n {ex.StackTrace}");
                throw; // Rethrow to see the actual error in the UI
            }
        }
    }
}
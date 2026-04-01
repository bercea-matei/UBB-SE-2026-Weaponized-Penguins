using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services.Interfaces;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace Boards_WP.ViewModels
{
    public partial class CommentViewModel : ObservableObject
    {
        [ObservableProperty]
        private Comment _commentData;

        [ObservableProperty]
        private bool _isReplyAreaVisible = false;

        [ObservableProperty]
        private string _replyText = string.Empty;

        public Action<Comment, string> ReplySubmitted { get; set; }

        public IRelayCommand UpvoteCommand { get; }
        public IRelayCommand DownvoteCommand { get; }
        public IRelayCommand ToggleReplyCommand { get; }
        public IRelayCommand SubmitReplyCommand { get; }

        private readonly ICommentsService _commentsService;
        private readonly UserSession _userSession;

        public SolidColorBrush UpvoteColor => CommentData?.UserCurrentVote == VoteType.Like 
            ? new SolidColorBrush(Colors.OrangeRed) 
            : new SolidColorBrush(Colors.Gray);

        public SolidColorBrush DownvoteColor => CommentData?.UserCurrentVote == VoteType.Dislike 
            ? new SolidColorBrush(Colors.CornflowerBlue) 
            : new SolidColorBrush(Colors.Gray);

        public CommentViewModel(Comment comment)
        {
            CommentData = comment;
            _commentsService = App.Services?.GetService<ICommentsService>();
            _userSession = App.Services?.GetService<UserSession>();

            UpvoteCommand = new RelayCommand(() => {
                if (_commentsService != null && _userSession != null)
                {
                    _commentsService.IncreaseScore(CommentData, _userSession.CurrentUser.UserID);
                    OnPropertyChanged(nameof(CommentData));
                    OnPropertyChanged(nameof(UpvoteColor));
                    OnPropertyChanged(nameof(DownvoteColor));
                }
            });

            DownvoteCommand = new RelayCommand(() => {
                if (_commentsService != null && _userSession != null)
                {
                    _commentsService.DecreaseScore(CommentData, _userSession.CurrentUser.UserID);
                    OnPropertyChanged(nameof(CommentData));
                    OnPropertyChanged(nameof(UpvoteColor));
                    OnPropertyChanged(nameof(DownvoteColor));
                }
            });

            ToggleReplyCommand = new RelayCommand(() => IsReplyAreaVisible = !IsReplyAreaVisible);

            SubmitReplyCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(ReplyText))
                {
                    ReplySubmitted?.Invoke(CommentData, ReplyText);
                    ReplyText = string.Empty;
                    IsReplyAreaVisible = false;
                }
            });
        }
    }
}
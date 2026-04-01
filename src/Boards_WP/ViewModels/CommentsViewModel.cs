using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services.Interfaces;

using System;
using Microsoft.Extensions.DependencyInjection;

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
                }
            });

            DownvoteCommand = new RelayCommand(() => {
                if (_commentsService != null && _userSession != null)
                {
                    _commentsService.DecreaseScore(CommentData, _userSession.CurrentUser.UserID);
                    OnPropertyChanged(nameof(CommentData));
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
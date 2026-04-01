using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;

namespace Boards_WP.ViewModels
{
    public partial class CommentViewModel : ObservableObject
    {
        [ObservableProperty]
        private Comment _commentData;

        [ObservableProperty]
        private bool _isReplyAreaVisible = false;

        [ObservableProperty]
        private string _replyText;

        public CommentViewModel(Comment comment)
        {
            _commentData = comment;
        }

        [RelayCommand]
        private void Upvote()
        {
            if (CommentData == null) return;
            CommentData.Score++;
            OnPropertyChanged(nameof(CommentData));
        }

        [RelayCommand]
        private void Downvote()
        {
            if (CommentData == null) return;
            CommentData.Score--;
            OnPropertyChanged(nameof(CommentData));
        }

        [RelayCommand]
        private void ToggleReply()
        {
            IsReplyAreaVisible = !IsReplyAreaVisible;
        }
    }
}
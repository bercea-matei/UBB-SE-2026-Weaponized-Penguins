using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Boards_WP.Data.Models;
using Boards_WP.ViewModels;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Boards_WP.Views.Pages
{
    public sealed partial class FullPostView : Page, INotifyPropertyChanged
    {
        public FullPostViewModel ViewModel { get; }

        private bool _isCommentAreaVisible = false;
        public bool IsCommentAreaVisible
        {
            get => _isCommentAreaVisible;
            set
            {
                _isCommentAreaVisible = value;
                OnPropertyChanged();
            }
        }

        public FullPostView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<FullPostViewModel>();
        }

        public static Visibility NullToVisibility(byte[] value) =>
            value != null ? Visibility.Visible : Visibility.Collapsed;

        public Visibility BooleanToVisibility(bool value) =>
            value ? Visibility.Visible : Visibility.Collapsed;

        public Visibility BooleanToInverseVisibility(bool value) =>
            value ? Visibility.Collapsed : Visibility.Visible;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Post clickedPost)
            {
                ViewModel.Initialize(clickedPost);
                this.Bindings.Update();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }

        private void ShowCommentBtn_Click(object sender, RoutedEventArgs e)
        {
            IsCommentAreaVisible = true;
            CommentInput.Focus(FocusState.Programmatic);
        }

        private void CancelComment_Click(object sender, RoutedEventArgs e)
        {
            IsCommentAreaVisible = false;
            ViewModel.NewCommentText = string.Empty;
        }

        private void PostComment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.NewCommentText)) return;

            var userSession = App.GetService<UserSession>();
            var commentsService = App.GetService<Data.Services.Interfaces.ICommentsService>();

            var newComment = new Comment
            {
                ParentPost = ViewModel.CurrentPost,
                ParentComment = null, // Root comment
                Owner = userSession.CurrentUser,
                Description = ViewModel.NewCommentText,
                Score = 0,
                Indentation = 0,
                CreationTime = DateTime.Now
            };

            try
            {
                commentsService.AddComment(newComment);

                // UI Cleanup
                IsCommentAreaVisible = false;
                ViewModel.NewCommentText = string.Empty;

                // Refresh post to show the new comment
                ViewModel.Initialize(ViewModel.CurrentPost);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to post comment: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
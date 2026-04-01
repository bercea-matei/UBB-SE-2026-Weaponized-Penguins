using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;

using System;

namespace Boards_WP.ViewModels
{
    public partial class PostPreviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private Post _postData;

        public Action<Post> OnDeleteRequested { get; set; }

        public PostPreviewViewModel(Post post)
        {
            PostData = post;
        }

        public string FormattedDate => PostData?.CreationTime.ToString("dd/MM/yyyy") ?? "";

        // Permission check for the UI
        public Visibility DeleteButtonVisibility =>
            (PostData?.Owner?.Username == "@Me" || PostData?.ParentCommunity?.Admin?.Username == "@Me")
            ? Visibility.Visible : Visibility.Collapsed;

        [RelayCommand]
        private void Upvote() { PostData.Score++; OnPropertyChanged(nameof(PostData)); }

        [RelayCommand]
        private void Downvote() { PostData.Score--; OnPropertyChanged(nameof(PostData)); }

        [RelayCommand]
        private void Delete() => OnDeleteRequested?.Invoke(PostData);

        [RelayCommand]
        private void OpenPost()
        {
            if (App.Current is App myApp && myApp.m_window is MainWindow mainWindow)
                mainWindow.NavigateToPage(typeof(Views.Pages.FullPostView), PostData);
        }
    }
}
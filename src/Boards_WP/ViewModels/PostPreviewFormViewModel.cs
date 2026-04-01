using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;

using System;

namespace Boards_WP.ViewModels
{
    // ObservableObject tells the UI to listen for property changes
    public partial class PostPreviewViewModel : ObservableObject
    {
        
        [ObservableProperty]
        private Post _postData;


        // constructor -> called by the .xaml.cs bridge
        public PostPreviewViewModel(Post post)
        {
            _postData = post;
        }


        public string FormattedDate
        {
            get
            {
                if (PostData != null)
                {
                    DateTime date = PostData.CreationTime;
                    return date.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }


        [RelayCommand]  // generates the UpvoteCommand
        private void Upvote()
        {
            if (PostData == null) return;
            PostData.Score++;
            OnPropertyChanged(nameof(PostData)); // refreshing the UI to display the new score
        }

        [RelayCommand]
        private void Downvote()
        {
            if (PostData == null) return;
            PostData.Score--;
            OnPropertyChanged(nameof(PostData));
        }


        // this function finds the MainWindow and tells it to switch to the FullPostView of the corresponding post
        [RelayCommand]
        private void OpenPost()
        {
            if (PostData == null) return;

            // navigation happens here
            if (App.Current is App myApp && myApp.m_window is MainWindow mainWindow)
            {
                mainWindow.NavigateToPage(typeof(Views.Pages.FullPostView), PostData);
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;
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
        private readonly ITagsRepository _tagsRepository;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UploadPostCommand))]
        private string _postTitle = string.Empty;

        [ObservableProperty]
        private string _postDescription = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTagCommand))]
        private string _currentTagText = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UploadPostCommand))]
        [NotifyCanExecuteChangedFor(nameof(AddTagCommand))]
        private Category? _selectedCategory;

        public ObservableCollection<Tag> AddedTags { get; } = new();

        public ObservableCollection<Category> AvailableCategories { get; } = new();

        public Community OriginCommunity { get; set; } = null!;

        public CreatePostViewModel(IPostsService postsService, INavigationService navigationService, UserSession userSession, ITagsRepository tagsRepository)
        {
            _postsService = postsService;
            _navigationService = navigationService;
            _userSession = userSession;
            _tagsRepository = tagsRepository;
            LoadCategories();
        }

        private void LoadCategories()
        {
            AvailableCategories.Clear();
            var categories = _tagsRepository.GetAllCategories();
            foreach (var c in categories) AvailableCategories.Add(c);
        }

        [RelayCommand(CanExecute = nameof(CanAddTag))]
        private void AddTag()
        {
            if (CanAddTag())
            {
                var newTag = new Tag
                {
                    TagName = CurrentTagText.Trim(),
                    CategoryBelongingTo = SelectedCategory!
                };
                AddedTags.Add(newTag);
                CurrentTagText = string.Empty;
                AddTagCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanAddTag() => AddedTags.Count < 10 && !string.IsNullOrWhiteSpace(CurrentTagText) && SelectedCategory != null;

        [RelayCommand]
        private void RemoveTag(Tag tagToRemove)
        {
            if (tagToRemove != null)
            {
                AddedTags.Remove(tagToRemove);
                AddTagCommand.NotifyCanExecuteChanged();
            }
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

            if (SelectedCategory != null && AddedTags.Count == 0 && !string.IsNullOrWhiteSpace(CurrentTagText))
            {
                AddTag();
            }

            if (AddedTags.Count == 0 && SelectedCategory != null)
            {
                var tag = new Tag { TagName = SelectedCategory.CategoryName, CategoryBelongingTo = SelectedCategory };
                AddedTags.Add(tag);
            }

            var finalTags = new System.Collections.Generic.List<Tag>();
            foreach (var tag in AddedTags)
            {
                _tagsRepository.AddTag(tag);
                finalTags.Add(tag);
            }

            newPost.Tags = finalTags;

            _postsService.AddPost(newPost);

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

        private bool CanUploadPost() => !string.IsNullOrWhiteSpace(PostTitle) && SelectedCategory != null;
    }
}

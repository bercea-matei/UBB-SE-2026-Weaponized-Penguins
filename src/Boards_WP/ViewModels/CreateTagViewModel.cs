using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.ObjectModel;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;

namespace Boards_WP.ViewModels;

public partial class CreateTagViewModel : ObservableObject
{
    private readonly ITagsRepository? _tagsRepository;

    [ObservableProperty]
    private string _tagName = string.Empty;

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ObservableCollection<Category> AvailableCategories { get; } = new();

    // Fired when a tag is created so CreatePostViewModel can add it
    public event Action<Tag>? TagCreated;
    public event Action? Cancelled;

    public CreateTagViewModel(ITagsRepository? tagsRepository = null)
    {
        _tagsRepository = tagsRepository;
        LoadCategories();
    }

    private void LoadCategories()
    {
        AvailableCategories.Clear();
        if (_tagsRepository != null)
        {
            var categories = _tagsRepository.GetAllCategories();
            foreach (var c in categories) AvailableCategories.Add(c);
        }
        else
        {
            // mock categories
            AvailableCategories.Add(new Category { CategoryID = 1, CategoryName = "Technology", ColorHex = "#FF4500" });
            AvailableCategories.Add(new Category { CategoryID = 2, CategoryName = "Science", ColorHex = "#0079D3" });
            AvailableCategories.Add(new Category { CategoryID = 3, CategoryName = "Gaming", ColorHex = "#46D160" });
        }
    }

    [RelayCommand]
    private void CreateTag()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(TagName))
        {
            ErrorMessage = "Tag name cannot be empty.";
            return;
        }

        if (SelectedCategory == null)
        {
            ErrorMessage = "Please select a category.";
            return;
        }

        var tag = new Tag
        {
            TagName = TagName,
            CategoryBelongingTo = SelectedCategory
        };

        try
        {
            _tagsRepository?.AddTag(tag);
            TagCreated?.Invoke(tag);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        Cancelled?.Invoke();
    }
}
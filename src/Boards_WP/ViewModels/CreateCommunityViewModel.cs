using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Boards_WP.ViewModels;

public partial class CreateCommunityViewModel : ObservableObject
{
    ICommunitiesService _communitiesService;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private byte[] picture;

    [ObservableProperty]
    private byte[] banner;


    [RelayCommand]
    private async Task CreateCommunity()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Name is required";
            return;
        }

        IsLoading = true;

        try
        {
            Community CreatedCommunity = new Community()
            {
                Name = Name,
                Description = Description,
                Picture = Picture,
                Banner = Banner,
                Admin = App.CurrentUser
            };

            _communitiesService.AddCommunity(CreatedCommunity);

            ErrorMessage = string.Empty;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong";
        }
        finally
        {
            IsLoading = false;
        }
    }
}

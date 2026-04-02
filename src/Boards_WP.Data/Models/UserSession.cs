using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Models;

public class UserSession
{
    public User CurrentUser { get; set; }

    public UserSession()
    {
        // Hardcoded user for development
        CurrentUser = new User
        {
            UserID = 3,
            Username = "@AlexBindiu",
            Email = "alex@boards.wp",
            PasswordHash = "hashed_pwd_1",
            Bio = "WinUI Dev & Penguin Commander",
            Status = "Online",
            AvatarUrl = "ms-appx:///Assets/DefaultAvatar.png"
        };
    }

    //login etc
}

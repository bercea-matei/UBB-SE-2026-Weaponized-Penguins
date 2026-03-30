using System;
using System.ComponentModel;

namespace Boards_WP.Data.Models;

public class Tag
{
<<<<<<< HEAD
    public int TagID { get; init; }
    public required Category CategoryBelongingTo { get; init; }
    public String TagName { get; set; } = String.Empty; 
    public String getColorHex => CategoryBelongingTo.ColorHex;
=======
    public int TagID { get; set; }
    public Category CategoryBelongingTo { get; set; }
    public string TagName { get; set; } = string.Empty; 
    public string ColorHex {  get; set; } = string.Empty;
>>>>>>> filip/community-view
}
using System;

namespace Boards_WP.Data.Models;

public class Tag
{
    public int TagID { get; set; }
    public int CategoryID { get; set; }
    public String TagName { get; set; } = String.Empty; 
}
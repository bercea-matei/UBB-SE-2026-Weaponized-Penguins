using System;

using namespace Boards_WP.Data.Models;

public class Tag
{
    public int tagID { get; set; }
    public int categoryID { get; set; }
    public String tagName { get; set; } = String.Empty;
}
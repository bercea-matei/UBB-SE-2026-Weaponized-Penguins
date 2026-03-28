using System;

namespace Boards_WP.Data.Models
{
    public class User
    {
        public int UserID { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Status { get; set; }
    }
}
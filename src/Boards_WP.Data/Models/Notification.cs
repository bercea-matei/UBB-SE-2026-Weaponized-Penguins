using System;

namespace Boards_WP.Data.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public DateTime CreationDate { get; init; } = DateTime.UtcNow;
        public Post RelatedPost { get; init; }
        public User Receiver { get; init; }
        public User Actor { get; init; }
        public NotificationType ActionType { get; init; }
        public bool IsRead { get; set; } = false;
    }
}

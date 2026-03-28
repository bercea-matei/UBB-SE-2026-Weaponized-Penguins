using System;

namespace Boards_WP.Data.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public DateTime CreationDate { get; set; }
        public int PostID { get; set; }
        public int ReceiverID { get; set; }
        public int ActorID { get; set; }
        public NotificationType ActionType { get; set; }
        public bool IsRead { get; set; }
    }
}

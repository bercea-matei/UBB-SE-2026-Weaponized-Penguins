using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;
using Boards_WP.Data.Services.Interfaces;

namespace Boards_WP.Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationRepository _notificationsRepo;

        public NotificationsService(INotificationRepository notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        public void AddNotification(Notification notification)
        {
            try
            {
                notification.NotificationID = _notificationsRepo.AddNotification(notification);
            }
            catch
            {
                throw new Exception("Failed to add notification");
            }
        }

        public void ReadNotification(Notification notification)
        {
            try
            {
                _notificationsRepo.MarkNotificationAsRead(notification.NotificationID);
            }
            catch
            {
                throw new Exception("Failed to mark notification as read");
            }
        }

        public List<Notification> GetNotificationsByUserID(int userID)
        {
            return _notificationsRepo.GetNotificationsByUserId(userID);
        }

        public string GetNotificationMessage(Notification notification)
        {
            string postTitle = notification.RelatedPost?.Title ?? "unknown post";
            string actorName = notification.Actor.Username;

            return notification.ActionType switch
            {
                NotificationType.CommentOnPost => $"{actorName} shared their thoughts on your post {postTitle}",
                NotificationType.ReplyToComment => $"{actorName} replied to your comment on post {postTitle}",
                NotificationType.PostDeleted => $"Your post '{postTitle}' was deleted",
                NotificationType.CommentDeleted => $"Your comment in '{postTitle}' was deleted",
                _ => string.Empty
            };
        }
    }
}
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

using Boards_WP.Data.Models
using Boards_WP.Data.Repositories.Interfaces;
using Boards_WP.Data.Services.Interfaces;


namespace Boards_WP.Data.Services : INotificationsService
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationRepository _notificationsRepo;

        public NotificationsService(INotificationRepository notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                notification.NotificationID = await _notificationsRepo.AddNotificationAsync(notification);

            }
            catch
            {
                throw new Exception("Failed to add notification");
            }
        }

        public async Task ReadNotificationAsync(Notification notification)
        {
            try
            {
                await _notificationsRepo.MarkNotificationAsReadAsync(notification);
            }
            catch
            {
                thow new Exception("Failed to mark notification as read");
            }
        }

        public async Task<List<Notification>> GetNotificationsByUserIDAsync(int userID)
        {
            return await _notificationsRepo.GetNotificationsByUserIdAsync(userID);
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
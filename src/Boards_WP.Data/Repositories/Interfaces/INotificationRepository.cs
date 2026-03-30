using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Boards_WP.Data.Models;


namespace Boards_WP.Data.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<int> AddNotificationAsync(Notification notification);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
    }
}
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

using Boards_WP.Data.Models
using Boards_WP.Data.Repositories.Interfaces;


namespace Boards_WP.Data.Services.Interfaces
{
    public interface INotificationsService
    {
        Task AddNotificationAsync(Notification notification);
        Task ReadNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByUserIDAsync(int userID);
        string GetNotificationMessage(Notification notification);
    }
}
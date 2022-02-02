using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStrategiesApi.Models
{
    public enum NotificationType
    {
        Created,
        Edited,
        Deleted,
        Info
    }

    public class Notification
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public string CreatorName { get; set; }
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
        public User User { get; set; }
        
        //Non database objects
        [NotMapped]
        public FirebaseNotificationData FirebaseNotificationData { get; set; }

        public Notification() { }

        /// <summary>
        /// Constructor for API-Notification Creation
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="user"></param>
        public Notification(string title, string content, NotificationType type, User user, string creator)
        {
            Title = title;
            Content = content;
            Seen = false;
            CreatorName = creator;
            Created = DateTime.UtcNow;
            Type = type;
            User = user;
        }

        public Notification(string title, string content, NotificationType type, string creator)
        {
            Title = title;
            Content = content;
            Seen = false;
            CreatorName = creator;
            Created = DateTime.UtcNow;
            Type = type;
        }
    }
}

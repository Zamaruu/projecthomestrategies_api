using System;
using System.Collections.Generic;
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
        public string Content { get; set; }
        public bool Seen { get; set; }
        public string CreatorName { get; set; }
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
        public User User { get; set; }

        public Notification() { }

        /// <summary>
        /// Constructor for API-Notification Creation
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="user"></param>
        public Notification(string content, NotificationType type, User user, string creator)
        {
            Content = content;
            Seen = false;
            CreatorName = creator;
            Created = DateTime.UtcNow;
            Type = type;
            User = user;
        }

        public Notification(string content, NotificationType type, string creator)
        {
            Content = content;
            Seen = false;
            CreatorName = creator;
            Created = DateTime.UtcNow;
            Type = type;
        }
    }
}

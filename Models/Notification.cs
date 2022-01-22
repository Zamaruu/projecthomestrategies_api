using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStrategiesApi.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public User User { get; set; }
    }
}

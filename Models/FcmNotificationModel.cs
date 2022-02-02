using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeStrategiesApi.Models
{
    public enum NotificationRoute
    {
        Notifications,
        Bills
    }

    public class FirebaseNotification
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("data")]
        public FirebaseNotificationData Data { get; set; }
    }

    public class FirebaseNotificationData
    {
        [JsonProperty("route")]
        public NotificationRoute Route { get; set; }
        [JsonProperty("args")]
        public string Arguments { get; set; }
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public FirebaseNotificationData Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }
}

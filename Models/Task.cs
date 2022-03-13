using System;

namespace HomeStrategiesApi.Models
{
    public enum TaskUrgency
    {
        Urgent,
        Normal,
        NotiImportant,
    }

    public class WorkTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskUrgency Urgency { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Creator { get; set; }
        public User UserId { get; set; }

    }
}

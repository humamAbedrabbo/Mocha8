using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AmsUser User { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public int? EntityId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Url
        {
            get
            {
                switch(NotificationType)
                {
                    case NotificationType.Asset:
                        return "Assets";
                    case NotificationType.Ticket:
                        return "Tickets";
                    case NotificationType.Task:
                        return "TodoTasks";
                    default:
                        return "Home";
                }
            }
        }
            
    }

    public enum NotificationType
    {
        None,
        Asset,
        Ticket,
        Task
    }
}

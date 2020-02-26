using System;
using System.Collections.Generic;

namespace AMS.Models
{
    public class TodoTask
    {
        public TodoTask()
        {
            Assignments = new List<Assignment>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int? TodoTaskTypeId { get; set; }
        public TodoTaskType TodoTaskType { get; set; }
        public WorkStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? PendingDate { get; set; }
        public bool MarkCompleted { get; set; }
        public int EstDuration { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}

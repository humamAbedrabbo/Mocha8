using System;
using System.Collections.Generic;

namespace AMS.Models
{
    public class Ticket
    {
        public Ticket()
        {
            TodoTasks = new List<TodoTask>();
            TicketAssets = new List<TicketAsset>();
            Assignments = new List<Assignment>();
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Summary { get; set; }
        public int CodeNumber { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public int? TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public WorkStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? PendingDate { get; set; }
        public bool MarkCompleted { get; set; }
        public int EstDuration { get; set; }
        public List<TodoTask> TodoTasks { get; set; }
        public List<TicketAsset> TicketAssets { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

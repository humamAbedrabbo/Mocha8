using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class Ticket
    {
        public Ticket()
        {
            StartDate = DateTime.Now;
            EstDuration = 1;
            DueDate = StartDate.AddDays(EstDuration);
            Status = WorkStatus.Open;
            TodoTasks = new List<TodoTask>();
            TicketAssets = new List<TicketAsset>();
            Assignments = new List<Assignment>();
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Summary")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Summary { get; set; }
        public int CodeNumber { get; set; }

        [Display(Name = "Code")]
        [StringLength(50, MinimumLength = 1)]
        public string Code { get; set; }

        [Display(Name = "Description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Client")]
        public int? ClientId { get; set; }
        public Client Client { get; set; }

        [Display(Name = "Ticket Type")]
        public int? TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name = "Status")]
        public WorkStatus Status { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Completion Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Cancellation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? CancellationDate { get; set; }

        [Display(Name = "Pending Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? PendingDate { get; set; }

        [Display(Name = "Mark Completed")]
        public bool MarkCompleted { get; set; }

        [Display(Name = "Est. Duration")]
        [Range(0, 1000)]
        public int EstDuration { get; set; }

        public bool IsOverdue => IsActive && (DueDate <= DateTime.Today);
        public bool CanBeCompleted => (Status == WorkStatus.Open);
        public bool CanBeCancelled => IsActive;
        public bool IsPending => PendingDate.HasValue;
        public bool IsActive => (Status == WorkStatus.Open || Status == WorkStatus.Pending);
        public int Delay => IsOverdue ? (int)(DateTime.Today - DueDate).TotalDays : 0;

        public List<TodoTask> TodoTasks { get; set; }
        public List<TicketAsset> TicketAssets { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<MetaFieldValue> Values { get; set; }
        public string Title => $"{Code}:{Summary}({Status.ToString()})";
        public string GroupTitle => $"{TicketType?.Name}";
    }
}

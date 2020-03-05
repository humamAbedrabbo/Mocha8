using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class TodoTask
    {
        public TodoTask()
        {
            StartDate = DateTime.Now;
            EstDuration = 1;
            DueDate = StartDate.AddDays(EstDuration);
            Status = WorkStatus.Open;
            Assignments = new List<Assignment>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Summary")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Summary { get; set; }

        [Display(Name = "Description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Ticket")]
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Display(Name = "Task Type")]
        public int? TodoTaskTypeId { get; set; }
        public TodoTaskType TodoTaskType { get; set; }

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

        public List<Assignment> Assignments { get; set; }
        public string Title => $"{Ticket?.Code}:{Summary}({Status.ToString()})";
        public string GroupTitle => $"{TodoTaskType?.Name}";
    }
}

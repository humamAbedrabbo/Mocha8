using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class TodoTaskType
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Task Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        
        [Display(Name = "Default Duration")]
        public int DefaultDuration { get; set; } = 1;

    }
}

using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Display(Name = "User Group")]
        public int? UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }

        [Display(Name = "User")]
        public int? UserId { get; set; }
        public AmsUser User { get; set; }

        [Display(Name = "Role Name")]
        [StringLength(50)]
        public string RoleName { get; set; }

        [Display(Name = "Ticket")]
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Display(Name = "User Group")]
        public int? TodoTaskId { get; set; }
        public TodoTask TodoTask { get; set; }
    }
}

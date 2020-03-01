using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class TicketType
    {
        public TicketType()
        {
            Tickets = new List<Ticket>();
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Ticket Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Code")]
        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Code { get; set; } = "TK";
        public List<Ticket> Tickets { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [Display(Name = "Default Duration")]
        public int DefaultDuration { get; set; } = 1;
        public List<Ticket> Tickets { get; set; }
        public List<MetaFieldValue> Values { get; set; }
        public IDictionary<string, MetaFieldValue> FieldValues => Values?.Where(x => x.Field != null).ToDictionary(x => x.Field?.Name, y => y);

    }
}

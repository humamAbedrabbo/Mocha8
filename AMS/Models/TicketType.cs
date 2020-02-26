using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Code { get; set; } = "TK";
        public List<Ticket> Tickets { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

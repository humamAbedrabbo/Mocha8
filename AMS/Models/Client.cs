using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class Client
    {
        public Client()
        {
            Assets = new List<Asset>();
            Tickets = new List<Ticket>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Client Type")]
        public int? ClientTypeId { get; set; }
        public ClientType ClientType { get; set; }

        [Display(Name = "Client Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        public List<Asset> Assets { get; set; }
        public List<Ticket> Tickets { get; set; }
        public string GroupTitle => $"{ClientType?.Name}";
    }
}

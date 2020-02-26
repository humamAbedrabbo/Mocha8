using System.Collections.Generic;
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
        public int? ClientTypeId { get; set; }
        public ClientType ClientType { get; set; }
        public string Name { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}

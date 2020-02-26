using System.Collections.Generic;

namespace AMS.Models
{
    public class ClientType
    {
        public ClientType()
        {
            Clients = new List<Client>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public List<Client> Clients { get; set; }
    }
}

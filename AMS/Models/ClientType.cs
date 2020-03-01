using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Client Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public List<Client> Clients { get; set; }
    }
}

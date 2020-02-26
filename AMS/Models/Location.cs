using System.Collections.Generic;

namespace AMS.Models
{
    public class Location
    {
        public Location()
        {
            Childs = new List<Location>();
            Assets = new List<Asset>();
            Tickets = new List<Ticket>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public int? LocationTypeId { get; set; }
        public LocationType LocationType { get; set; }
        public int? ParentId { get; set; }
        public Location Parent { get; set; }
        public List<Location> Childs { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}

using System.Collections.Generic;

namespace AMS.Models
{
    public class LocationType
    {
        public LocationType()
        {
            Locations = new List<Location>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public List<Location> Locations { get; set; }
    }
}

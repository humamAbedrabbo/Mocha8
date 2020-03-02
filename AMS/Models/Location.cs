using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Location Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Location Type")]
        public int? LocationTypeId { get; set; }
        public LocationType LocationType { get; set; }

        [Display(Name = "Parent Location")]
        public int? ParentId { get; set; }
        public Location Parent { get; set; }

        public List<Location> Childs { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Ticket> Tickets { get; set; }
        public string GroupTitle => $"{LocationType?.Name}";
    }
}

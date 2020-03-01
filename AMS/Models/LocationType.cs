using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Location Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public List<Location> Locations { get; set; }
    }
}

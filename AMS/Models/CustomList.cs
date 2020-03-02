using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class CustomList
    {
        public CustomList()
        {
            Items = new List<CustomListItem>();
            MetaFields = new List<MetaField>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "List Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public List<CustomListItem> Items { get; set; }
        public List<MetaField> MetaFields { get; set; }
    }
}

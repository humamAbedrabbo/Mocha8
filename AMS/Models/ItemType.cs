using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class ItemType
    {
        public ItemType()
        {
            Items = new List<AssetItem>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Item Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public List<AssetItem> Items { get; set; }
    }
}

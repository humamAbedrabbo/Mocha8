using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class AssetType
    {
        public AssetType()
        {
            Assets = new List<Asset>();
            Values = new List<MetaFieldValue>();
        }
        
        public int Id { get; set; }
        
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Asset Type Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Code")]
        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Code { get; set; } = "A";

        public List<Asset> Assets { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

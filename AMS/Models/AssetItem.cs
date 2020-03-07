using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class AssetItem
    {
        public int Id { get; set; }

        [Display(Name = "Asset")]
        [Required]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        [Display(Name = "Item Type")]
        public int? ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }

        [Display(Name = "Part Number")]
        [StringLength(50)]
        public string PartNumber { get; set; }
    }
}

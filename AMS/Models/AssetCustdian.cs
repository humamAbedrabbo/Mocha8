using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class AssetCustdian
    {
        public int Id { get; set; }

        [Display(Name = "Asset")]
        [Required]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        [Display(Name = "User")]
        public int? UserId { get; set; }
        public AmsUser User { get; set; }

        [Display(Name = "Custodian Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Role Name")]
        [StringLength(50, MinimumLength = 1)]
        public string RoleName { get; set; }
    }
}

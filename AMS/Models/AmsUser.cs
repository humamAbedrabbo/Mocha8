using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class AmsUser : IdentityUser<int>
    {
        public AmsUser()
        {
            PictureUrl = "/images/avatars/user.jpg";
            AssetCustodians = new List<AssetCustdian>();
            Members = new List<Member>();
            Assignments = new List<Assignment>();
            
        }

        [Display(Name = "Tenant")]
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [PersonalData]
        [Display(Name = "Display Name")]
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [PersonalData]
        [Display(Name = "Company")]
        [StringLength(50)]
        public string Company { get; set; }

        [PersonalData]
        [Display(Name = "Job Title")]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [PersonalData]
        [Display(Name = "Picture")]
        [DataType(DataType.ImageUrl)]
        [StringLength(250)]
        public string PictureUrl { get; set; }

        public List<AssetCustdian> AssetCustodians { get; set; }
        public List<Member> Members { get; set; }
        public List<Assignment> Assignments { get; set; }
        public string Title => DisplayName;
    }
}

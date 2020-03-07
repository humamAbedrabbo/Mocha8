using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [PersonalData]
        public string DisplayName { get; set; }

        [PersonalData]
        public string Company { get; set; }

        [PersonalData]
        public string JobTitle { get; set; }

        [PersonalData]
        public string PictureUrl { get; set; }

        public List<AssetCustdian> AssetCustodians { get; set; }
        public List<Member> Members { get; set; }
        public List<Assignment> Assignments { get; set; }
        public string Title => DisplayName;
    }
}

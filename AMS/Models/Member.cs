using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Display(Name = "User Group")]
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }


        [Display(Name = "User")]
        public int UserId { get; set; }
        public AmsUser User { get; set; }

        [Display(Name = "Member Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        public string Title => $"{Name}:{User?.DisplayName}";
        public string GroupTitle => $"{UserGroup?.Name}";
    }
}

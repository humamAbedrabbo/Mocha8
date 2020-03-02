using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class CustomListItem
    {
        public int Id { get; set; }

        [Display(Name = "List Name")]
        public int CustomListId { get; set; }
        public CustomList CustomList { get; set; }

        [Display(Name = "Item Key")]
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Key { get; set; }

        [Display(Name = "Item Value")]
        [StringLength(100)]
        public string Value { get; set; }
        public string Title => $"{Key}:{Value}";
        public string GroupTitle => $"{CustomList?.Name}";
    }
}

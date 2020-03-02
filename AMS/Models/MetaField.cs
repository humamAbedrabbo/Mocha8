using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class MetaField
    {
        public MetaField()
        {
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        
        [Display(Name = "Field Type")]
        public FieldType FieldType { get; set; }
        
        [Display(Name = "List Name")]
        public int? CustomListId { get; set; }
        public CustomList CustomList { get; set; }
        
        [Display(Name = "Field Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)] 
        public string Name { get; set; }

        public List<MetaFieldValue> Values { get; set; }
    }
}

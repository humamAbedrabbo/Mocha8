using System.Collections.Generic;

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
        public FieldType FieldType { get; set; }
        public int? CustomListId { get; set; }
        public CustomList CustomList { get; set; }
        public string Name { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

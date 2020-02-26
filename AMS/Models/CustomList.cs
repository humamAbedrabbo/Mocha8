using System.Collections.Generic;

namespace AMS.Models
{
    public class CustomList
    {
        public CustomList()
        {
            Items = new List<CustomListItem>();
            MetaFields = new List<MetaField>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public List<CustomListItem> Items { get; set; }
        public List<MetaField> MetaFields { get; set; }
    }
}

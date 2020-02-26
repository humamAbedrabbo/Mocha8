using System.Collections.Generic;

namespace AMS.Models
{
    public class ItemType
    {
        public ItemType()
        {
            Items = new List<AssetItem>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public List<AssetItem> Items { get; set; }
    }
}

using System.Collections.Generic;

namespace AMS.Models
{
    public class AssetType
    {
        public AssetType()
        {
            Assets = new List<Asset>();
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } = "A";
        public List<Asset> Assets { get; set; }
        public List<MetaFieldValue> Values { get; set; }
    }
}

using System.Collections.Generic;

namespace AMS.Models
{
    public class Asset
    {
        public Asset()
        {
            Childs = new List<Asset>();
            TicketAssets = new List<TicketAsset>();
            Items = new List<AssetItem>();
            Custodians = new List<AssetCustdian>();
            Values = new List<MetaFieldValue>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public int CodeNumber { get; set; }
        public string Code { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public int? AssetTypeId { get; set; }
        public AssetType AssetType { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int? ParentId { get; set; }
        public Asset Parent { get; set; }
        public bool IsOn { get; set; }
        public List<Asset> Childs { get; set; }
        public List<TicketAsset> TicketAssets { get; set; }
        public List<AssetItem> Items { get; set; }
        public List<AssetCustdian> Custodians { get; set; }
        public List<MetaFieldValue> Values { get; set; }

    }
}

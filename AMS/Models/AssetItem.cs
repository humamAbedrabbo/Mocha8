namespace AMS.Models
{
    public class AssetItem
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int? ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
        public string PartNumber { get; set; }
    }
}

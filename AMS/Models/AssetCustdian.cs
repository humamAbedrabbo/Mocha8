namespace AMS.Models
{
    public class AssetCustdian
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int? UserId { get; set; }
        public AmsUser User { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
    }
}

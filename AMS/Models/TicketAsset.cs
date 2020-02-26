namespace AMS.Models
{
    public class TicketAsset
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}

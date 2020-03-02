using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class TicketAsset
    {
        public int Id { get; set; }

        [Display(Name = "Asset")]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        [Display(Name = "Ticket")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}

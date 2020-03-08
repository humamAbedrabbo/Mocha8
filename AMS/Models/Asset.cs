using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [Display(Name = "Asset Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public int CodeNumber { get; set; }

        [Display(Name = "Code")]
        [StringLength(50, MinimumLength = 1)]
        public string Code { get; set; }

        [Display(Name = "Client")]
        public int? ClientId { get; set; }
        public Client Client { get; set; }

        [Display(Name = "Asset Type")]
        [Required]
        public int? AssetTypeId { get; set; }
        public AssetType AssetType { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name = "Parent Asset")]
        public int? ParentId { get; set; }
        public Asset Parent { get; set; }

        [Display(Name = "On/Off")]
        public bool IsOn { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public List<Asset> Childs { get; set; }
        public List<TicketAsset> TicketAssets { get; set; }
        public List<AssetItem> Items { get; set; }
        public List<AssetCustdian> Custodians { get; set; }
        public List<MetaFieldValue> Values { get; set; }
        public string Active => IsOn ? "On" : "Off";
        public string Title => $"{Code}:{Name}({Active})";
        public string GroupTitle => $"{AssetType?.Name}";

        public IEnumerable<Ticket> ActiveTickets => TicketAssets.Select(x => x.Ticket).Where(x => x.IsActive);
        public IDictionary<string, MetaFieldValue> FieldValues => Values?.Where(x => x.Field != null).ToDictionary(x => x.Field?.Name, y => y);
    }
}

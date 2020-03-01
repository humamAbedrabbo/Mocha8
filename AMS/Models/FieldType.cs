using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public enum FieldType
    {
        [Display(Name = "Short Text")]
        Text,

        [Display(Name = "Long Text")]
        LargeText,

        [Display(Name = "Number")]
        Number,

        [Display(Name = "Decimal Number")]
        Decimal,

        [Display(Name = "Date")]
        Date,

        [Display(Name = "Date/Time")]
        DateTime,

        [Display(Name = "Url Link")]
        Url,

        [Display(Name = "Boolean (true/false)")]
        Boolean,

        [Display(Name = "Pick Item from List")]
        ListItem
    }
}

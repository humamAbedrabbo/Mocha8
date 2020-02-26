using System;

namespace AMS.Models
{
    public class MetaFieldValue
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public MetaField Field { get; set; }
        public int? AssetTypeId { get; set; }
        public AssetType AssetType { get; set; }
        public int? AssetId { get; set; }
        public Asset Asset { get; set; }
        public int? TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string Value { get; set; }
        public string UrlValue { get => Value; set => Value = value; }

        public DateTime? DateValue
        {
            get
            {
                if(string.IsNullOrEmpty(Value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDateTime(Value);
                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString("yyyy-MM-dd") : null;
            }
        }

        public DateTime? DateTimeValue
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDateTime(Value);
                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null;
            }
        }

        public int? NumberValue
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(Value);
                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString() : null;
            }
        }

        public double? DecimalValue
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDouble(Value);
                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString() : null;
            }
        }
    }
}

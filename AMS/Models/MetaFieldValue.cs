using System;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public string UrlValue { get => Value; set => Value = value; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
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
                    if (this.Field?.FieldType == FieldType.Date || this.Field?.FieldType == FieldType.DateTime)
                        return Convert.ToDateTime(Value);
                    else 
                        return null;
                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString("yyyy-MM-dd") : null;
            }
        }

        [Display(Name = "Date/Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.DateTime)]
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
                    if (this.Field?.FieldType == FieldType.Date || this.Field?.FieldType == FieldType.DateTime)
                        return Convert.ToDateTime(Value);
                    else
                        return null;

                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null;
            }
        }

        [Display(Name = "Number")]
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
                    if (this.Field?.FieldType == FieldType.Number)
                        return Convert.ToInt32(Value);
                    else
                        return null;

                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString() : null;
            }
        }

        [Display(Name = "Decimal")]
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
                    if (this.Field?.FieldType == FieldType.Decimal)
                        return Convert.ToDouble(Value);
                    else
                        return null;

                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString() : null;
            }
        }

        [Display(Name = "True/False")]
        public bool? BooleanValue
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                {
                    return null;
                }
                else
                {
                    if (this.Field?.FieldType == FieldType.Boolean)
                        return Convert.ToBoolean(Value);
                    else
                        return null;

                }
            }
            set
            {
                Value = value.HasValue ? value.Value.ToString() : null;
            }
        }

        public override string ToString()
        {
            return $"{Field?.Name}/{Value}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }
}

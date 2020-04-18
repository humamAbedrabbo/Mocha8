﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool IsArchived { get; set; } = false;
        public int? RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public int? DocumentId { get; set; }
        public int? Version { get; set; }
        public string Url { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
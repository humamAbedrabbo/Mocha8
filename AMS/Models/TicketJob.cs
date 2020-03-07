﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class TicketJob
    {
        public TicketJob()
        {
            TicketJobTaskTypes = new List<TicketJobTaskType>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Display(Name = "Ticket Type")]
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }

        [Display(Name = "Asset Type")]
        public int AssetTypeId { get; set; }
        public AssetType AssetType { get; set; }

        [Display(Name = "Client")]
        public int? ClientId { get; set; }
        public Client Client { get; set; }

        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name = "Owner")]
        public int OwnerId { get; set; }
        public AmsUser Owner { get; set; }

        [Display(Name = "User Group")]
        public int? UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }

        [Display(Name = "Summary")]
        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Summary { get; set; }

        [Display(Name = "Job Id")]
        public string JobId { get; set; }

        public bool IsOn { get; set; }

        [Display(Name = "Schedule")]
        public string CronSchedule { get; set; }

        public List<TicketJobTaskType> TicketJobTaskTypes { get; set; }

        [Display(Name = "Task Types")]
        public IEnumerable<int> TaskTypes { get; set; }

    }

    public class TicketJobTaskType
    {
        public int TicketJobId { get; set; }
        public TicketJob TicketJob { get; set; }
        public int TodoTaskTypeId { get; set; }
        public TodoTaskType TodoTaskType { get; set; }
    }
}
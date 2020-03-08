using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class Tenant
    {
        public Tenant()
        {
            Users = new List<AmsUser>();
            Roles = new List<AmsRole>();
            Clients = new List<Client>();
            LocationTypes = new List<LocationType>();
            AssetTypes = new List<AssetType>();
            TicketTypes = new List<TicketType>();
            TodoTaskTypes = new List<TodoTaskType>();
            Locations = new List<Location>();
            Assets = new List<Asset>();
            Tickets = new List<Ticket>();
            TodoTasks = new List<TodoTask>();
            UserGroups = new List<UserGroup>();
            ClientTypes = new List<ClientType>();
            MetaFields = new List<MetaField>();
            CustomLists = new List<CustomList>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tenant Name")]
        public string Name { get; set; }
        public List<AmsUser> Users { get; set; }
        public List<AmsRole> Roles { get; set; }
        public List<Client> Clients { get; set; }
        public List<LocationType> LocationTypes { get; set; }
        public List<AssetType> AssetTypes { get; set; }
        public List<TicketType> TicketTypes { get; set; }
        public List<TodoTaskType> TodoTaskTypes { get; set; }
        public List<Location> Locations { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TodoTask> TodoTasks { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public List<ClientType> ClientTypes { get; set; }
        public List<MetaField> MetaFields { get; set; }
        public List<CustomList> CustomLists { get; set; }
    }
}

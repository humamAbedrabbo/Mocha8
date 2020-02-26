using System.Collections.Generic;

namespace AMS.Models
{
    public class UserGroup
    {
        public UserGroup()
        {
            Members = new List<Member>();
            Assignments = new List<Assignment>();
        }
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}

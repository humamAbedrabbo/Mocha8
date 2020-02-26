namespace AMS.Models
{
    public class TodoTaskType
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
    }
}

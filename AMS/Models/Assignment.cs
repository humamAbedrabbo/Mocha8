namespace AMS.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int? UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public int? UserId { get; set; }
        public AmsUser User { get; set; }
        public string RoleName { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int? TodoTaskId { get; set; }
        public TodoTask TodoTask { get; set; }
    }
}

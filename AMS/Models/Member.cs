namespace AMS.Models
{
    public class Member
    {
        public int Id { get; set; }
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public int UserId { get; set; }
        public AmsUser User { get; set; }
        public string Name { get; set; }
        public string Title => $"{Name}:{User?.DisplayName}";
        public string GroupTitle => $"{UserGroup?.Name}";
    }
}

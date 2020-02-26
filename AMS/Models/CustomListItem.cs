namespace AMS.Models
{
    public class CustomListItem
    {
        public int Id { get; set; }
        public int CustomListId { get; set; }
        public CustomList CustomList { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Title => $"{Key}:{Value}";
        public string GroupTitle => $"{CustomList?.Name}";
    }
}

namespace GoodDay.BLL.DTO
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public bool Blocked { get; set; }
        public string ContactName { get; set; }
        public string UserId { get; set; }
        public string UserFriendId { get; set; }
    }
}

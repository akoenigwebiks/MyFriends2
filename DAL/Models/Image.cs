namespace MyFriends2.DAL.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; } // Foreign key

        // Navigation property
        public User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriends2.DAL.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] ImageFile { get; set; }  // Stores the image as a byte array

        [StringLength(255)]
        public string FileName { get; set; }  // To store the file name of the image

        [ForeignKey("User")]
        public int UserId { get; set; }  // Foreign key

        public User User { get; set; }  // Navigation property
    }
}

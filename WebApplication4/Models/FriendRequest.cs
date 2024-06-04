using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Status { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
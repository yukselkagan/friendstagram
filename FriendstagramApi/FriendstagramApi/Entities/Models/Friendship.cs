using System.ComponentModel.DataAnnotations.Schema;

namespace FriendstagramApi.Entities.Models
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FriendId { get; set; }
        [ForeignKey("FriendId")]
        public User Friend { get; set; }
    }
}

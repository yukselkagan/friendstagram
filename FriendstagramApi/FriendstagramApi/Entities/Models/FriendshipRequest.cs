using System.ComponentModel.DataAnnotations.Schema;

namespace FriendstagramApi.Entities.Models
{
    public class FriendshipRequest
    {
        public int FriendshipRequestId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
    }
}

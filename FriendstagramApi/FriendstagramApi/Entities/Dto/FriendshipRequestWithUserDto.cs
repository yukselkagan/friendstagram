using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Entities.Dto
{
    public class FriendshipRequestWithUserDto
    {
        public int FriendshipRequestId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
    }
}

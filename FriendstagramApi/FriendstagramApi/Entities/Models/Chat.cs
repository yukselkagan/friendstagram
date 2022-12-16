using System;

namespace FriendstagramApi.Entities.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

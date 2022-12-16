using FriendstagramApi.Entities.Models;
using System;

namespace FriendstagramApi.Entities.Dto
{
    public class ChatDto
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int FriendId { get; set; }
        public UserDto Friend { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

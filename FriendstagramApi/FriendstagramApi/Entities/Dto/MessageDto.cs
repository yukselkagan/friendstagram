using FriendstagramApi.Entities.Models;
using System;

namespace FriendstagramApi.Entities.Dto
{
    public class MessageDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public UserDto Sender { get; set; }
        public int ReceiverId { get; set; }
        public UserDto Receiver { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

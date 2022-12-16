using FriendstagramApi.Entities.Models;
using System.Collections.Generic;

namespace FriendstagramApi.Services.Interfaces
{
    public interface IMessageService
    {
        public Message SendMessage(int userId, int receiverId, string messageText);
        public List<Message> GetMessagesForChat(int userId, int friendId);
        public List<Chat> GetChats(int userId);
    }
}

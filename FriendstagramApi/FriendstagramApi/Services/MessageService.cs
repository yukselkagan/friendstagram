using FriendstagramApi.Data;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriendstagramApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Message SendMessage(int userId, int receiverId, string messageText)
        {
            var message = new Message()
            {
                SenderId = userId,
                ReceiverId = receiverId,
                MessageText = messageText,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.MessageRepository.Insert(message);
            _unitOfWork.Save();

            ProcessForChat(message);
            
            return message;
        }

        public List<Message> GetMessagesForChat(int userId, int friendId)
        {
            var messages = _unitOfWork.MessageRepository
                .Get(filter: x => x.SenderId == userId && x.ReceiverId == friendId
                || x.SenderId == friendId && x.ReceiverId == userId).ToList();

            return messages;
        }

        public List<Chat> GetChats(int userId)
        {
            var chatList = _unitOfWork.ChatRepository
                .Get(filter: x => x.UserId == userId, orderBy : x => x.OrderByDescending(x => x.UpdatedAt), 
                includeProperties: "User,Friend").ToList();

            return chatList;
        }

        private void ProcessForChat(Message message)
        {
            var existingChat = _unitOfWork.ChatRepository
                .Get(filter: x => x.UserId == message.SenderId && x.FriendId == message.ReceiverId ).FirstOrDefault();
            var existingChatForFriend = _unitOfWork.ChatRepository
                .Get(filter: x => x.UserId == message.ReceiverId && x.FriendId == message.SenderId ).FirstOrDefault();

            if(existingChat == null)
            {
                var chatForUser = new Chat() { UserId = message.SenderId, FriendId = message.ReceiverId, UpdatedAt = DateTime.Now };
                var chatForFriend = new Chat() { UserId = message.ReceiverId, FriendId = message.SenderId, UpdatedAt = DateTime.Now };

                _unitOfWork.ChatRepository.Insert(chatForUser);
                _unitOfWork.ChatRepository.Insert(chatForFriend);
                _unitOfWork.Save();
            }
            else
            {
                existingChat.UpdatedAt = DateTime.Now;
                existingChatForFriend.UpdatedAt = DateTime.Now;
                _unitOfWork.Save();
            }
        }





    }
}

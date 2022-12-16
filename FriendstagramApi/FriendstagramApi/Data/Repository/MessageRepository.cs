using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

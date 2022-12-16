using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

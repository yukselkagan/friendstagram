using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        public FriendshipRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

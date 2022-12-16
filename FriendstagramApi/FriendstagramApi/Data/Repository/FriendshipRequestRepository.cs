using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class FriendshipRequestRepository : GenericRepository<FriendshipRequest>, IFriendshipRequestRepository
    {
        public FriendshipRequestRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

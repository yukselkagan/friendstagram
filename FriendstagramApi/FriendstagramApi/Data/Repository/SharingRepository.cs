using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class SharingRepository : GenericRepository<Sharing>, ISharingRepository
    {
        public SharingRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

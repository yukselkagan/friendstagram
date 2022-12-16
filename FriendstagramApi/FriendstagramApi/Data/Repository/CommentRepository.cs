using FriendstagramApi.Data.Repository.Interfaces;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Data.Repository
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(FriendstagramContext context) : base(context)
        {
        }
    }
}

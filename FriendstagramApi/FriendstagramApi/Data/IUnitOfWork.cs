using FriendstagramApi.Data.Repository.Interfaces;

namespace FriendstagramApi.Data
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public ISharingRepository SharingRepository { get; set; }
        public IFriendshipRepository FriendshipRepository { get; set; }
        public IFriendshipRequestRepository FriendshipRequestRepository { get; set; }
        public ICommentRepository CommentRepository { get; set; }
        public IMessageRepository MessageRepository { get; set; }
        public IChatRepository ChatRepository { get; set; }

        public void Save();
    }
}

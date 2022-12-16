using FriendstagramApi.Data.Repository.Interfaces;

namespace FriendstagramApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FriendstagramContext _context;
        public IUserRepository UserRepository { get; set; }
        public ISharingRepository SharingRepository { get; set; }
        public IFriendshipRepository FriendshipRepository { get; set; }
        public IFriendshipRequestRepository FriendshipRequestRepository { get; set; }
        public ICommentRepository CommentRepository { get; set; }
        public IMessageRepository MessageRepository { get; set; }
        public IChatRepository ChatRepository { get; set; }

        public UnitOfWork(FriendstagramContext context, IUserRepository userRepository,
            ISharingRepository sharingRepository,
            IFriendshipRepository friendshipRepository,
            IFriendshipRequestRepository friendshipRequestRepository,
            ICommentRepository commentRepository,
            IMessageRepository messageRepository, 
            IChatRepository chatRepository)
        {
            _context = context;
            this.UserRepository = userRepository;
            this.SharingRepository = sharingRepository;
            this.FriendshipRepository = friendshipRepository;
            this.FriendshipRequestRepository = friendshipRequestRepository;
            this.CommentRepository = commentRepository;
            this.MessageRepository = messageRepository;
            this.ChatRepository = chatRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}

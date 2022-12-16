using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;
using System.Collections.Generic;

namespace FriendstagramApi.Services.Interfaces
{
    public interface ICommentService
    {
        public List<Comment> GetCommentsForSharing(int sharingId);
        public Comment CommentToSharing(int userId, CommentToSharingDto commentToSharingDto);
    }
}

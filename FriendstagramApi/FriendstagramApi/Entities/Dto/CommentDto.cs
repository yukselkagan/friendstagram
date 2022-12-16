using System;

namespace FriendstagramApi.Entities.Dto
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int SharingId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

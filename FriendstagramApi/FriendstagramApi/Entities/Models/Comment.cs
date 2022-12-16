using System;

namespace FriendstagramApi.Entities.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SharingId { get; set; }
        public Sharing Sharing { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

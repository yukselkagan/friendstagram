using System.ComponentModel.DataAnnotations.Schema;

namespace FriendstagramApi.Entities.Models
{
    public class Sharing
    {
        public int SharingId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Path { get; set; }
        public string SharingText { get; set; }
    }
}

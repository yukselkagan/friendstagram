using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Entities.Dto
{
    public class SharingDto
    {
        public int SharingId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public string Path { get; set; }
        public string SharingText { get; set; }
    }
}

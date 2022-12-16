using FriendstagramApi.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FriendstagramApi.Entities.Dto
{
    public class FriendshipRequestDto
    {
        public int FriendshipRequestId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int FriendId { get; set; }
    }
}

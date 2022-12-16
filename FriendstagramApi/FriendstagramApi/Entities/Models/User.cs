using System.ComponentModel.DataAnnotations.Schema;

namespace FriendstagramApi.Entities.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        
    }
}

using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace FriendstagramApi.Services.Interfaces
{
    public interface IUserService
    {
        public object Register(User user);
        public object Login(User userLogin);
        public User GetUser(int userId);
        public User GetUserByUserName(string userName);
        public object ChangeProfileInformation(int userId, UserChangeProfileInformationDto profileChanges);
        public object ChangeProfilePicture(int userId, IFormFile file);
        public string CreatePasswordHash(string password);
    }
}

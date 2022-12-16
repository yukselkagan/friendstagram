using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Services.General.Token
{
    public interface ITokenManager
    {
        public string CreateAccessToken(User user);
    }
}

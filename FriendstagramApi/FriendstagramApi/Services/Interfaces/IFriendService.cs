using FriendstagramApi.Entities.Models;
using System.Collections.Generic;

namespace FriendstagramApi.Services.Interfaces
{
    public interface IFriendService
    {
        //public bool CheckForHaveFriendshipRequest(int userId, int profileId);
        public string CheckForFriendship(int userId, int profileId);
        public FriendshipRequest SendFriendRequest(int userId, int friendId);
        public FriendshipRequest RevokeFriendshipRequestByFriendId(int userId, int friendId);
        public List<FriendshipRequest> GetFriendshipRequestListForUser(int userId);
        public List<User> GetFriendSuggestionList(int userId);
        public FriendshipRequest AnswerFriendshipRequest(int userId, int friendshipRequestId, bool accepted);
        public List<Friendship> GetFriendships(int userId);
        public object RemoveFriendship(int userId, int friendId);



    }
}

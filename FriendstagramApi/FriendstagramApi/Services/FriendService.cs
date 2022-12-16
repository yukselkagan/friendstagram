using FriendstagramApi.Data;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriendstagramApi.Services
{
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FriendService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public bool CheckForHaveFriendshipRequest(int userId, int profileId)
        //{
        //    var friendshipRequest = _unitOfWork.FriendshipRequestRepository
        //        .Get(filter: x => x.UserId == userId && x.FriendId == profileId).FirstOrDefault();

        //    if (friendshipRequest != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public string CheckForFriendship(int userId, int profileId)
        {
            var friendship = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == profileId).FirstOrDefault();
            var friendshipRequest = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == profileId).FirstOrDefault();

            if(friendship != null)
            {
                return "Friend";
            }
            else if (friendshipRequest != null)
            {
                return "Request";
            }
            else
            {
                return "None";
            }
        }

        public FriendshipRequest SendFriendRequest(int userId, int friendId)
        {
            var existingFriendship = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == friendId).FirstOrDefault();
            var existingRequest = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == friendId).FirstOrDefault();
            var oppositeFrienshipRequest = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.UserId == friendId && x.FriendId == userId).FirstOrDefault();

            if (existingRequest == null)
            {
                if(existingFriendship == null)
                {
                    if(oppositeFrienshipRequest == null)
                    {
                        FriendshipRequest friendshipRequest = new FriendshipRequest()
                        {
                            UserId = userId,
                            FriendId = friendId
                        };

                        _unitOfWork.FriendshipRequestRepository.Insert(friendshipRequest);
                        _unitOfWork.Save();

                        return friendshipRequest;
                    }
                    else
                    {
                        var response = AnswerFriendshipRequest(userId, oppositeFrienshipRequest.FriendshipRequestId, true);
                        return response;
                    }                   
                }
                else
                {
                    throw new Exception("Already friend");
                }               
            }
            else
            {
                throw new Exception("Friendship request already sent");
            }
        }

        public FriendshipRequest RevokeFriendshipRequestByFriendId(int userId, int friendId)
        {
            var friendshipRequest = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == friendId).FirstOrDefault();

            if(friendshipRequest != null)
            {
                var response = RevokeFriendshipRequest(friendshipRequest);
                return response;
            }
            else
            {
                throw new Exception("Can not find friendship request");
            }
        }

        private FriendshipRequest RevokeFriendshipRequest(FriendshipRequest friendshipRequest)
        {
            _unitOfWork.FriendshipRequestRepository.Delete(friendshipRequest);
            _unitOfWork.Save();

            return friendshipRequest;
        }

        public List<FriendshipRequest> GetFriendshipRequestListForUser(int userId)
        {
            var friendshipRequestList = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.FriendId == userId, includeProperties: "User").ToList();

            return friendshipRequestList;
        }

        public List<User> GetFriendSuggestionList(int userId)
        {
            var friendIdList = GetFriendIdList(userId);
            var nonFriendList = GetNonFriendUserListWithFriendIdList(userId, friendIdList);
            var friendSuggestionList = SelectRandomFriendSuggestion(nonFriendList);

            return friendSuggestionList;
        }

        private List<int> GetFriendIdList(int userId)
        {
            var friendIdList = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == userId).Select(x => x.FriendId).ToList();

            return friendIdList;
        }

        private List<User> GetNonFriendUserListWithFriendIdList(int userId, List<int> friendIdList)
        {
            var nonFriendList = _unitOfWork.UserRepository
                .Get(filter: x => !friendIdList.Contains(x.UserId) && x.UserId != userId ).ToList();

            return nonFriendList;
        }

        private List<User> SelectRandomFriendSuggestion(List<User> nonFriendList)
        {
            List<User> randomList = new List<User>();
            for (int i = 1; i <= 5; i++)
            {
                if (nonFriendList.Count > 0)
                {
                    var rnd = new Random();

                    int index = rnd.Next(nonFriendList.Count);
                    randomList.Add(nonFriendList[index]);
                    nonFriendList.RemoveAt(index);
                }
            }

            return randomList;
        }


        public FriendshipRequest AnswerFriendshipRequest(int userId, int friendshipRequestId, bool accepted)
        {
            var targetFriendshipRequest = _unitOfWork.FriendshipRequestRepository.GetById(friendshipRequestId);
            var isRequestValid = targetFriendshipRequest.FriendId == userId;

            if (isRequestValid)
            {
                if (accepted)
                {
                    var newFriendship = new Friendship()
                    {
                        UserId = targetFriendshipRequest.UserId,
                        FriendId = targetFriendshipRequest.FriendId
                    };
                    var newFriendshipReverse = new Friendship()
                    {
                        UserId = targetFriendshipRequest.FriendId,
                        FriendId = targetFriendshipRequest.UserId
                    };

                    _unitOfWork.FriendshipRequestRepository.Delete(targetFriendshipRequest);
                    CleanAdditionalFriendshipRequests(targetFriendshipRequest);

                    _unitOfWork.FriendshipRepository.Insert(newFriendship);
                    _unitOfWork.FriendshipRepository.Insert(newFriendshipReverse);
                    _unitOfWork.Save();

                    return targetFriendshipRequest;
                }
                else
                {
                    _unitOfWork.FriendshipRequestRepository.Delete(targetFriendshipRequest);
                    _unitOfWork.Save();

                    return targetFriendshipRequest;
                }
            }
            else
            {
                throw new Exception("Invalid friendship request");
            }
        }


        private void CleanAdditionalFriendshipRequests(FriendshipRequest friendshipRequest)
        {
            var requests = _unitOfWork.FriendshipRequestRepository
                .Get(filter: x => x.UserId == friendshipRequest.UserId && x.FriendId == friendshipRequest.FriendId 
                || x.FriendId == friendshipRequest.UserId && x.UserId == friendshipRequest.FriendId );

            foreach(var item in requests)
            {
                _unitOfWork.FriendshipRequestRepository.Delete(item);
                _unitOfWork.Save();
            }
        }

        public List<Friendship> GetFriendships(int userId)
        {
            var friendships = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == userId, includeProperties: "Friend,User" ).ToList();

            return friendships;
        }

        public object RemoveFriendship(int userId, int friendId)
        {
            var friendship = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == userId && x.FriendId == friendId).FirstOrDefault();
            var friendshipReverse = _unitOfWork.FriendshipRepository
                .Get(filter: x => x.UserId == friendId && x.FriendId == userId).FirstOrDefault();

            if(friendship != null)
            {
                _unitOfWork.FriendshipRepository.Delete(friendship);
                _unitOfWork.FriendshipRepository.Delete(friendshipReverse);
                _unitOfWork.Save();

                return friendship;
            }
            else
            {
                throw new Exception("Can not find friendship");
            }
        }



    }
}

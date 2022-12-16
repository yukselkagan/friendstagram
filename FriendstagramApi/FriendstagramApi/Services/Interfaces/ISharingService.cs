using FriendstagramApi.Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FriendstagramApi.Services.Interfaces
{
    public interface ISharingService
    {
        public List<Sharing> GetAllSharings();
        public List<Sharing> GetSharingsForMainPage(int userId);
        public List<Sharing> GetSharingsByUserId(int userId);
        public Sharing GetSharing(int sharingId);
        public string UploadImage(int userId, IFormFile file, string sharingText);
    }
}

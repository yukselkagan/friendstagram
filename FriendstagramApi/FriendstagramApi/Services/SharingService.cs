using FriendstagramApi.Data;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Services.General;
using FriendstagramApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace FriendstagramApi.Services
{
    public class SharingService : ISharingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFriendService _friendService;
        public SharingService(IUnitOfWork unitOfWork, IFriendService friendService)
        {
            _unitOfWork = unitOfWork;
            _friendService = friendService;
        }

        public List<Sharing> GetAllSharings()
        {
            var sharingList = _unitOfWork.SharingRepository
                .Get(includeProperties: "User", orderBy: x => x.OrderByDescending(x => x.SharingId)).ToList();
            return sharingList;
        }

        public List<Sharing> GetSharingsForMainPage(int userId)
        {
            var friendIdList = _friendService.GetFriendships(userId).Select(x => x.FriendId).ToList();

            var sharingList = _unitOfWork.SharingRepository
                .Get(filter: x => friendIdList.Contains(x.UserId), orderBy : x => x.OrderByDescending(x => x.SharingId)).ToList();

            return sharingList;
        }

        public List<Sharing> GetSharingsByUserId(int userId)
        {
            var sharingList = _unitOfWork.SharingRepository
                .Get(filter: x => x.UserId == userId, orderBy: x => x.OrderByDescending(x => x.SharingId))
                .ToList();

            return sharingList;
        }

        public Sharing GetSharing(int sharingId)
        {
            var sharing = _unitOfWork.SharingRepository
                .Get(filter: x => x.SharingId == sharingId, includeProperties: "User").FirstOrDefault();
            return sharing;
        }

        public string UploadImage(int userId, IFormFile file, string sharingText)
        {
            var folderName = Path.Combine("Stock", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileExtension = Path.GetExtension(fileName);
                if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                {
                    string createdUniqueNamae = GeneralManager.CreateUniqueName();
                    fileName = createdUniqueNamae + fileExtension;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var responsePath = SaveUploadedImageToDatabase(userId, fileName, sharingText);

                    return responsePath;
                }
                else
                {
                    throw new Exception("Extension Problem");
                }
            }
            else
            {
                throw new Exception("Empty File");
            }
        }

        private string SaveUploadedImageToDatabase(int userId, string path, string sharingText)
        {
            Sharing newSharing = new Sharing
            {
                Path = path,
                SharingText = sharingText,
                UserId = userId
            };

            _unitOfWork.SharingRepository.Insert(newSharing);
            _unitOfWork.Save();

            return newSharing.Path;
        }


    }
}

using FriendstagramApi.Data;
using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Services.General;
using FriendstagramApi.Services.General.Token;
using FriendstagramApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace FriendstagramApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenManager _tokenManager;
        public UserService(IUnitOfWork unitOfWork, ITokenManager tokenManager)
        {
            _unitOfWork = unitOfWork;
            _tokenManager = tokenManager;
        }


        public object Register(User user)
        {
            var sameEmailUser = _unitOfWork.UserRepository.Get(x => x.Email == user.Email).FirstOrDefault();
            var sameUserNameUser = _unitOfWork.UserRepository.Get(x => x.UserName == user.UserName).FirstOrDefault();

            if(sameEmailUser == null  && sameUserNameUser == null)
            {
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();

                var tokenString = _tokenManager.CreateAccessToken(user);
                var userToken = new UserToken()
                {
                    User = user,
                    AccessToken = tokenString
                };

                return userToken;
            }
            else
            {
                if (sameEmailUser != null)
                {
                    throw new Exception("Email already taken");
                }
                else
                {
                    throw new Exception("User name already taken");
                }               
            }
        }


        public object Login(User userLogin)
        {
            var user = _unitOfWork.UserRepository.Get(x => x.Email == userLogin.Email).FirstOrDefault();
            if(user == null)
            {
                throw new Exception("Email or password is wrong");
            }
            var passwordVerified = VerifyPasswordHash(userLogin.Password, user.PasswordHash);

            if (passwordVerified)
            {
                var tokenString = _tokenManager.CreateAccessToken(user);
                var userToken = new UserToken()
                {
                    User = user,
                    AccessToken = tokenString
                };

                return userToken;
            }
            else
            {
                throw new Exception("Email or password is wrong");
            }
        }


        public User GetUser(int userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            return user;
        }

        public User GetUserByUserName(string userName)
        {
            var user = _unitOfWork.UserRepository.Get(filter: x => x.UserName == userName).FirstOrDefault();
            if(user == null)
            {
                throw new Exception("Can not find user");
            }

            return user;
        }

        public object ChangeProfileInformation(int userId, UserChangeProfileInformationDto profileChanges)
        {
            string displayName = profileChanges.DisplayName;
            if (displayName.Length >= 3)
            {
                var user = _unitOfWork.UserRepository.GetById(userId);
                user.DisplayName = displayName;
                _unitOfWork.Save();

                return user;
            }
            else
            {
                throw new Exception("Display name need to be minimum 3 letter");
            }
        }

        public object ChangeProfilePicture(int userId, IFormFile file)
        {
            var folderName = Path.Combine("Stock", "ProfileImages");
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
                    var responseUser = SaveChangedProfilePictureToDatabase(userId, fileName);
                    return responseUser;
                }
                else
                {
                    throw new Exception("Wrong file type");
                }
            }
            else
            {
                throw new Exception("Empty file");
            }
        }

        private object SaveChangedProfilePictureToDatabase(int userId, string path)
        {
            User user = GetUser(userId);
            user.ProfilePicture = path;
            _unitOfWork.Save();

            return user;
        }

        public string CreatePasswordHash(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);

            var hashedPassword = Encoding.ASCII.GetString(md5data);
            return hashedPassword;
        }

        private bool VerifyPasswordHash(string password, string inputPasswordHash)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var passwordHash = Encoding.ASCII.GetString(md5data);

            if(inputPasswordHash == passwordHash)
            {
                return true;
            }

            return false;
        }



    }
}

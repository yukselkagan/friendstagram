using AutoMapper;
using FriendstagramApi.Data;
using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Extensions;
using FriendstagramApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FriendstagramApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public ActionResult Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                var mappedUser = _mapper.Map<User>(userRegisterDto);
                mappedUser.PasswordHash = _userService.CreatePasswordHash(userRegisterDto.Password);

                var response = _userService.Register(mappedUser);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login(UserLoginDto userLoginDto)
        {
            try
            {
                var mappedUser = _mapper.Map<User>(userLoginDto);
                var response = _userService.Login(mappedUser);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserSelf")]
        public ActionResult GetUserSelf()
        {
            try
            {
                List<Claim> claims = HttpContext.User.Claims.ToList();
                string claimOfIdValue = claims.FirstOrDefault(x => x.Type == "id").Value;

                int userId;
                int.TryParse(claimOfIdValue, out userId);

                if(userId > 0)
                {
                    var responseUser = _userService.GetUser(userId);
                    if(responseUser == null)
                    {
                        throw new Exception("Can not find user");
                    }
                    var userDto = _mapper.Map<UserDto>(responseUser);

                    return Ok(responseUser);
                }
                else
                {
                    throw new Exception("Can not read token");
                }              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

        [HttpGet("GetUserByUserName/{userName}")]
        [AllowAnonymous]
        public ActionResult GetUserByUserName(string userName)
        {
            try
            {
                var responseUser = _userService.GetUserByUserName(userName);
                var userDto = _mapper.Map<UserDto>(responseUser);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserById/{userId}")]
        [AllowAnonymous]
        public ActionResult GetUserById(int userId)
        {
            try
            {
                var responseUser = _userService.GetUser(userId);
                var userDto = _mapper.Map<UserDto>(responseUser);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ChangeProfileInformation")]
        public ActionResult ChangeProfileInformation(UserChangeProfileInformationDto profileChanges)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var response = _userService.ChangeProfileInformation(userId, profileChanges);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }      
        }

        [HttpPost("ChangeProfilePicture")]
        public ActionResult ChangeProfilePicture()
        {           
            try
            {
                var userId = HttpContext.User.ReadUserId();
                var file = Request.Form.Files[0];

                var response = _userService.ChangeProfilePicture(userId, file);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

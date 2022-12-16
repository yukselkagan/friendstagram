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

namespace FriendstagramApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FriendController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFriendService _friendService;
        public FriendController(IFriendService friendService, IMapper mapper)
        {
            _friendService = friendService;
            _mapper = mapper;
        }

        [HttpGet("GetFriendSuggestionList")]
        public ActionResult<IEnumerable<UserDto>> GetFriendSuggestionList()
        {
            var userId = HttpContext.User.ReadUserId();
            var friendSuggestionList = _friendService.GetFriendSuggestionList(userId);
            
            var friendSuggestionListDto = _mapper.Map<List<UserDto>>(friendSuggestionList);

            return Ok(friendSuggestionListDto);
        }

        //[HttpPost("CheckForHaveFriendshipRequest")]
        //public ActionResult CheckForHaveFriendshipRequest([FromBody] int profileId)
        //{
        //    try
        //    {
        //        int userId = HttpContext.User.ReadUserId();
        //        var responseHaveRequest = _friendService.CheckForHaveFriendshipRequest(userId, profileId);

        //        return Ok(responseHaveRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("CheckForFriendship")]
        public ActionResult CheckForFriendship([FromBody] int profileId)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var responseFriendshipStatus = _friendService.CheckForFriendship(userId, profileId);

                return Ok(responseFriendshipStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SendFriendRequest")]
        public ActionResult SendFriendRequest([FromBody] int friendId)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                if (friendId == 0) { throw new Exception("Can not find user"); }

                var response = _friendService.SendFriendRequest(userId, friendId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RevokeFriendshipRequestByFriendId")]
        public ActionResult RevokeFriendshipRequestByFriendId([FromBody] int friendId)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var response = _friendService.RevokeFriendshipRequestByFriendId(userId, friendId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFriendshipRequestListForUser")]
        public ActionResult<List<FriendshipRequestWithUserDto>> GetFriendshipRequestListForUser()
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var response = _friendService.GetFriendshipRequestListForUser(userId);
                var mappedFriendshipRequestList = _mapper.Map<List<FriendshipRequestWithUserDto>>(response);

                return Ok(mappedFriendshipRequestList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AnswerFriendshipRequest")]
        public ActionResult<FriendshipRequestDto> AnswerFriendshipRequest(AnswerFriendshipRequestDto answerFriendshipRequestDto)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();

                var responseFriendshipRequest = _friendService.AnswerFriendshipRequest(userId, answerFriendshipRequestDto.FriendshipRequestId, answerFriendshipRequestDto.Accepted);
                var friendshipRequestDto = _mapper.Map<FriendshipRequestDto>(responseFriendshipRequest);

                return Ok(friendshipRequestDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("GetFriendships/{userId}")]
        public ActionResult GetFriendships(int userId)
        {
            try
            {
                userId = (userId == 0) ? HttpContext.User.ReadUserId() : userId;
                var response = _friendService.GetFriendships(userId);
                var friendshipDtoList = _mapper.Map<List<FriendshipDto>>(response);

                return Ok(friendshipDtoList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoveFriendship")]
        public ActionResult RemoveFriendship([FromBody] int friendId)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var response = _friendService.RemoveFriendship(userId, friendId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

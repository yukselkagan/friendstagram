using AutoMapper;
using FriendstagramApi.Entities.Dto;
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
    public class CommentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        public CommentController(IMapper mapper, ICommentService commentService)
        {
            _mapper = mapper;
            _commentService = commentService;
        }


        [HttpGet("GetCommentsForSharing")]
        public ActionResult<List<CommentDto>> GetCommentsForSharing(int sharingId)
        {
            try
            {
                var commentList = _commentService.GetCommentsForSharing(sharingId);
                var commentDtoList = _mapper.Map<List<CommentDto>>(commentList);

                return Ok(commentDtoList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CommentToSharing")]
        public ActionResult<CommentDto> CommentToSharing(CommentToSharingDto commentToSharingDto)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();

                var responseComment = _commentService.CommentToSharing(userId, commentToSharingDto);
                var commentDto = _mapper.Map<CommentDto>(responseComment);

                return Ok(commentDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



    }
}

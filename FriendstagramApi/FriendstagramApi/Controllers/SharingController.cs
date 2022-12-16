using AutoMapper;
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
    public class SharingController : ControllerBase
    {
        private readonly ISharingService _sharingService;
        private readonly IMapper _mapper;
        public SharingController(ISharingService sharingService, IMapper mapper)
        {
            _sharingService = sharingService;
            _mapper = mapper;
        }


        [HttpGet("GetAllSharings")]
        public ActionResult GetAllSharings()
        {
            var sharingList = _sharingService.GetAllSharings();
            return Ok(sharingList);
        }

        [HttpGet("GetSharingsForMainPage")]
        public ActionResult GetSharingsForMainPage()
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var sharingList = _sharingService.GetSharingsForMainPage(userId);

                return Ok(sharingList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSharingsByUserId")]
        public ActionResult<List<Sharing>> GetSharingsByUserId(int userId)
        {
            try
            {
                var response = _sharingService.GetSharingsByUserId(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSharing/{sharingId}")]
        [AllowAnonymous]
        public ActionResult GetSharing(int sharingId)
        {
            try
            {
                var sharing = _sharingService.GetSharing(sharingId);
                var sharingDto = _mapper.Map<SharingDto>(sharing);

                return Ok(sharingDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UploadImage")]
        [DisableRequestSizeLimit]
        public ActionResult UploadImage()
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();

                var file = Request.Form.Files[0];
                var sharingText = Request.Form["sharingText"][0];

                var responsePath = _sharingService.UploadImage(userId, file, sharingText);
                UploadImageDto uploadImageDto = new UploadImageDto(responsePath);

                return Ok(uploadImageDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        public MessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }


        [HttpPost("SendMessage")]
        public ActionResult SendMessage(SendMessageDto sendMessageDto)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var response = _messageService.SendMessage(userId, sendMessageDto.ReceiverId, sendMessageDto.MessageText);
                var mappedMessage = _mapper.Map<MessageDto>(response);

                return Ok(mappedMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("GetMessagesForChat")]
        public ActionResult<List<MessageDto>> GetMessagesForChat(int friendId)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var responseMessages = _messageService.GetMessagesForChat(userId, friendId);
                var mappedMessages = _mapper.Map<List<MessageDto>>(responseMessages);

                return Ok(mappedMessages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetChats")]
        public ActionResult GetChats()
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var chatList = _messageService.GetChats(userId);
                var mappedChatList = _mapper.Map<List<ChatDto>>(chatList);

                return Ok(mappedChatList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

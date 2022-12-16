using AutoMapper;
using FriendstagramApi.Data;
using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;
using FriendstagramApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriendstagramApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<Comment> GetCommentsForSharing(int sharingId)
        {
            var commentList = _unitOfWork.CommentRepository
                .Get(filter: x => x.SharingId == sharingId, includeProperties: "User").ToList();

            return commentList;
        }

        public Comment CommentToSharing(int userId, CommentToSharingDto commentToSharingDto)
        {
            Comment newComment = new Comment();

            _mapper.Map(commentToSharingDto, newComment);
            newComment.UserId = userId;
            newComment.CreatedAt = DateTime.Now;
            newComment.UpdatedAt = DateTime.Now;

            _unitOfWork.CommentRepository.Insert(newComment);
            _unitOfWork.Save();

            return newComment;
        }



    }
}

using AutoMapper;
using FriendstagramApi.Entities.Dto;
using FriendstagramApi.Entities.Models;

namespace FriendstagramApi.Services.General.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentToSharingDto>().ReverseMap();

            CreateMap<Friendship, FriendshipDto>().ReverseMap();

            CreateMap<FriendshipRequest, FriendshipRequestDto>().ReverseMap();
            CreateMap<FriendshipRequest, FriendshipRequestWithUserDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Message, SendMessageDto>().ReverseMap();

            CreateMap<Chat, ChatDto>().ReverseMap();

            CreateMap<Sharing, SharingDto>().ReverseMap();

        }
    }
}

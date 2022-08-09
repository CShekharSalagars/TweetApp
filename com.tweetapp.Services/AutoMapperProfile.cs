using AutoMapper;
using com.tweetapp.Models.Models;
using com.tweetapp.Services.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, RegisterUserDto>().ReverseMap();

            CreateMap<TweetModel, TweetDto>().ReverseMap();


            CreateMap<UserModel, UserDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();


            CreateMap<LikeModel, LikeDto>()
                .ForMember(dest => dest.IsLiked, opt => opt.MapFrom(src => src.isLiked))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

            CreateMap<TweetDto, TweetModel>()
                .ForMember(dest => dest.likeModels, opt => opt.MapFrom(src => src.tweetLikes))
                .ReverseMap();
            CreateMap<AddTweetDto, TweetModel>().ReverseMap();
            CreateMap<AddLikeDto, LikeModel>().ReverseMap();
        }
    }
}

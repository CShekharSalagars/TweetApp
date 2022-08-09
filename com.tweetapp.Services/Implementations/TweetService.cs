using AutoMapper;
using com.tweetapp.Entity;
using com.tweetapp.Models.Models;
using com.tweetapp.Services.DTOS;
using com.tweetapp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.Implementations
{
    public class TweetService : ITweetService
    {
        private readonly TweetAppContext dbContext;
        private readonly IMapper mapper;
        public readonly IHttpContextAccessor httpContextAccessor;
        public TweetService(TweetAppContext _dbContext, IMapper _mapper, IHttpContextAccessor _httpContextAccessor)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
        }


        /// <summary>
        /// function will add new post
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<TweetDto>>> AddNewPost(AddTweetDto addTweetDto, string userName)
        {
            var serviceResponse = new ServiceResponse<List<TweetDto>>();
            try
            {
                TweetModel tweetModel = mapper.Map<TweetModel>(addTweetDto);
                var userModel = GetUserByName(userName);
                if (userModel != null)
                {
                    tweetModel.User = userModel;
                }
                dbContext.Tweets.Add(tweetModel);
                await dbContext.SaveChangesAsync();
                serviceResponse = await GetTweetList();
                return serviceResponse;
            }
            catch (Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Something went wrong";
                return serviceResponse;
            }
        }

        /// <summary>
        /// Function will return all the tweets
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<TweetDto>>> GetAllTweets()
        {
            return await GetTweetList();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<List<UserDto>>> GetAllUsers()
        {
            var servicesResponse = new ServiceResponse<List<UserDto>>();
            var userEntiries = await dbContext.User.ToListAsync();
            servicesResponse.Success = userEntiries.Any();
            servicesResponse.Data = userEntiries.Select(x => mapper.Map<UserDto>(x)).ToList();
            return servicesResponse;

        }

        public async Task<ServiceResponse<List<UserDto>>> SearchByUserName(string userName)
        {
            var servicesResponse = new ServiceResponse<List<UserDto>>();
            var userEntiries = await dbContext.User.Where(x => x.UserName == userName).ToListAsync();
            servicesResponse.Success = userEntiries.Any();
            servicesResponse.Data = userEntiries.Select(x => mapper.Map<UserDto>(x)).ToList();
            return servicesResponse;
        }

        /// <summary>
        /// Get tweets by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<List<TweetDto>>> GetTweetByUserName(string userName)
        {
            var serviceResponse = new ServiceResponse<List<TweetDto>>();
            var tweetEntities = await dbContext.Tweets.Where(x => x.User.UserName == userName)
                                                .Include(x => x.likeModels)
                                                .Include(x => x.replyModels)
                                                .ToListAsync();
            serviceResponse.Success = tweetEntities.Any();
            serviceResponse.Data = tweetEntities.Select(x => mapper.Map<TweetDto>(x)).ToList();
            return serviceResponse;
        }

        /// <summary>
        /// function will update the tweet
        /// </summary>
        /// <param name="addTweetDto"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<TweetDto>> UpdateTweet(AddTweetDto addTweetDto, string userName, int Id)
        {
            var serviceResponse = new ServiceResponse<TweetDto>();
            try
            {
                var tweetEntity = await dbContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == Id);
                if (tweetEntity == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Tweet not found";
                    return serviceResponse;
                }
                tweetEntity.TweetMessage = addTweetDto.TweetMessage;
                tweetEntity.TweetModifyDate = DateTime.Now;
                var userModel = GetUserByName(userName);
                if (userModel != null)
                {
                    tweetEntity.User = userModel;
                }
                await dbContext.SaveChangesAsync();
                serviceResponse.Data = mapper.Map<TweetDto>(tweetEntity);
                return serviceResponse;
            }
            catch (Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Something went wrong";
                return serviceResponse;
            }
        }

        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<string>> DeleteTweet(string userName, int Id)
        {
            var serviceResponse = new ServiceResponse<string>();
            var userId = GetUserId();
            var tweetEntity = await dbContext.Tweets.Where(x => x.User.UserName == userName && x.TweetId == Id).FirstOrDefaultAsync();
            if (tweetEntity == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No tweet found to delete or you dont have permission to delete this tweet";
                return serviceResponse;
            }
            dbContext.Tweets.Remove(tweetEntity);
            dbContext.SaveChanges();
            serviceResponse.Success = true;
            serviceResponse.Data = "Tweet deleted";
            return serviceResponse;
        }

        /// <summary>
        /// Save like on the tweet
        /// </summary>
        /// <param name="addLikeDto"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> SaveTweetLike(AddLikeDto addLikeDto, int Id)
        {
            var serviceResponse = new ServiceResponse<string>();
            var tweetEntity = await dbContext.Tweets.Where(x => x.TweetId == Id).FirstOrDefaultAsync();
            if (tweetEntity == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Tweet not found";
                return serviceResponse;
            }
            var contextUserId = GetUserId();
            var userLike = await dbContext.Likes.Where(x => x.User.UserId == contextUserId && x.Tweet.TweetId == Id).FirstOrDefaultAsync();
            if (userLike == null)
            {
                var userModel = await dbContext.User.Where(x => x.UserId == contextUserId).FirstOrDefaultAsync();
                var likeModel = new LikeModel();
                likeModel.Tweet = tweetEntity;
                if (userModel != null)
                {
                    likeModel.User = userModel;
                }
                likeModel.isLiked = addLikeDto.isLiked;
                likeModel.LikeDate = DateTime.Now;
                dbContext.Likes.Add(likeModel);
                serviceResponse.Success = false;
                serviceResponse.Message = "Tweet response added";
                await dbContext.SaveChangesAsync();
                return serviceResponse;
            }
            userLike.isLiked = addLikeDto.isLiked;
            userLike.LikeDate = DateTime.Now;
            await dbContext.SaveChangesAsync();
            serviceResponse.Success = false;
            serviceResponse.Message = "Tweet response added";
            return serviceResponse;
        }

        /// <summary>
        /// Add reply to the tweet
        /// </summary>
        /// <param name="addLikeDto"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<string>> SaveTweetReply(AddReplyDto addReplyDto, int Id)
        {
            var serviceResponse = new ServiceResponse<string>();
            var tweetEntity = await dbContext.Tweets.Where(x => x.TweetId == Id).FirstOrDefaultAsync();
            if (tweetEntity == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Tweet not found";
                return serviceResponse;
            }
            var contextUserId = GetUserId();
            var userModel = await dbContext.User.Where(x => x.UserId == contextUserId).FirstOrDefaultAsync();
            var replyModel = new ReplyModel();
            replyModel.ReplyMessage = addReplyDto.ReplyMessage;
            replyModel.ReplyDate = DateTime.Now;
            replyModel.User = userModel;
            replyModel.UserId = contextUserId;
            replyModel.Tweet = tweetEntity;
             dbContext.Replies.Add(replyModel);
            await dbContext.SaveChangesAsync();

            serviceResponse.Success = true;
            serviceResponse.Message = "Replied to a tweet";
            return serviceResponse;

        }

        #region[Private Methods]
        private int GetUserId()
        {
            var userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userID != null)
            {
                return Convert.ToInt32(userID);
            }
            return 0;
        }

        private UserModel GetUserByName(string userName)
        {
            var userModel = dbContext.User.Where(x => x.UserName == userName).FirstOrDefault();
            if (userModel == null)
            {
                return null;
            }
            return userModel;
        }

        private async Task<ServiceResponse<List<TweetDto>>> GetTweetList()
        {
            var serviceResponse = new ServiceResponse<List<TweetDto>>();
            var tweetEntities = await dbContext.Tweets.Include(x => x.likeModels).Include(x => x.replyModels).ToListAsync();
            serviceResponse.Success = tweetEntities.Any();
            serviceResponse.Data = tweetEntities.Select(x => mapper.Map<TweetDto>(x)).ToList();
            return serviceResponse;
        }

        #endregion
    }
}

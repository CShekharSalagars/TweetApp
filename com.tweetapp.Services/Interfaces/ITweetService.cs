using com.tweetapp.Services.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.Interfaces
{
    public interface ITweetService
    {
        /// <summary>
        /// Function will return all the tweets
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<TweetDto>>> GetAllTweets();

        /// <summary>
        /// Get all  users
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<UserDto>>> GetAllUsers();

        /// <summary>
        /// Get all users by username
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<UserDto>>> SearchByUserName(string userName);

        /// <summary>
        /// function will add new tweet
        /// </summary>
        /// <param name="addTweetDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<List<TweetDto>>> AddNewPost(AddTweetDto addTweetDto, string userName);


        /// <summary>
        /// Get tweets by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ServiceResponse<List<TweetDto>>> GetTweetByUserName(string userName);

        /// <summary>
        /// function will update the tweet
        /// </summary>
        /// <param name="addTweetDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<TweetDto>> UpdateTweet(AddTweetDto addTweetDto,string userName, int Id);

       
        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> DeleteTweet(string userName, int Id);

        /// <summary>
        ///  Add likes for the tweet
        /// </summary>
        /// <param name="addLikeDto"></param>
        /// <param name="userName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> SaveTweetLike(AddLikeDto addLikeDto, int Id);

        /// <summary>
        /// Save tweet reply
        /// </summary>
        /// <param name="addReplyDto"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> SaveTweetReply(AddReplyDto addReplyDto, int Id);


    }
}

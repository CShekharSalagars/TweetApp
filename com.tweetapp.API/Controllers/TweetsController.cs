using com.tweetapp.Services.DTOS;
using com.tweetapp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace com.tweetapp.API.Controllers
{
    [Authorize]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetService tweetService;

        public TweetsController(ITweetService _tweetService)
        {
            tweetService = _tweetService;
        }

        [HttpGet]
        [Route("tweets/all")]
        public async Task<IActionResult> GetAll()
        {
            var serviceResponse = await tweetService.GetAllTweets();
            return Ok(serviceResponse);
        }

        [HttpGet]
        [Route("tweets/users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var serviceResponse = await tweetService.GetAllUsers();
            return Ok(serviceResponse);
        }

        [HttpGet]
        [Route("tweets/user/search/{userName}")]
        public async Task<IActionResult> SearchByUserName(string userName)
        {
            var serviceResponse = await tweetService.SearchByUserName(userName);
            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("tweets/{userName}/add")]
        public async Task<IActionResult> AddNewPost(AddTweetDto tweetDto, string userName)
        {
            var serviceResponse = await tweetService.AddNewPost(tweetDto, userName);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }


        [HttpGet]
        [Route("tweets/user/{userName}")]
        public async Task<IActionResult> GetTweetsByUser(string userName)
        {
            var serviceResponse = await tweetService.GetTweetByUserName(userName);
            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("tweets/{userName}/update/{Id}")]
        public async Task<IActionResult> UpdateTweet(AddTweetDto addTweetDto, string userName, int Id)
        {
            var serviceResponse = await tweetService.UpdateTweet(addTweetDto, userName, Id);
            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("tweets/{userName}/delete/{Id}")]
        public async Task<IActionResult> Delete(string userName, int Id)
        {
            var serviceResponse = await tweetService.DeleteTweet(userName, Id);
            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("tweets/like/{Id}")]
        public async Task<IActionResult> LikeTweet(AddLikeDto addLikeDto, int Id)
        {
            var serviceResponse = await tweetService.SaveTweetLike(addLikeDto, Id);
            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }


        [HttpPost]
        [Route("tweets/reply/{Id}")]
        public async Task<IActionResult> ReplyToTweet(AddReplyDto replyDto, int Id)
        {
            var serviceResponse = await tweetService.SaveTweetReply(replyDto, Id);
            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.DTOS
{
    public class TweetDto
    {
        public string TweetMessage { get; set; }
        public DateTime TweetDate { get; set; }
        public List<LikeDto> tweetLikes { get; set; }
        public List<ReplyDto> tweetReplies { get; set; }

    }

    public class LikeDto
    {
        public int UserName { get; set; }
        public bool IsLiked { get; set; }

    }

    public class ReplyDto
    {
        public string RepliedBy { get; set; }
        public string ReplyMessage { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Models.Models
{
    [Table("Tweet")]
    public class TweetModel
    {
        [Key]
        public int TweetId { get; set; }
        public string TweetMessage { get; set; }
        public DateTime TweetDate { get; set; }
        public DateTime TweetModifyDate { get; set; }
        public UserModel User { get; set; }
        public List<LikeModel> likeModels { get; set; }
        public List<ReplyModel> replyModels { get; set; }
    }
}

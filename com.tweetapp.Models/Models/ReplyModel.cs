using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Models.Models
{
    [Table("Replies")]
    public class ReplyModel
    {
        [Key]
        public int Id { get; set; }
        public string ReplyMessage { get; set; }
        public TweetModel Tweet { get; set; }
        public UserModel User { get; set; }
        public int UserId { get; set; }
        public DateTime ReplyDate { get; set; }
    }
}

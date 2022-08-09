using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Models.Models
{
    [Table("Like")]
    public class LikeModel
    {
        public int Id { get; set; }
        public TweetModel Tweet { get; set; }
        public UserModel User { get; set; }
        public DateTime LikeDate { get; set; }

        public bool isLiked { get; set; }

    }
}

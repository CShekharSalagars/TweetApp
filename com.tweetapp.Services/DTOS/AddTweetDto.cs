using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.DTOS
{
    public class AddTweetDto
    {
        [Required]
        public string TweetMessage { get; set; }

        public DateTime TweetDate { get; set; } = DateTime.Now;
    }
}

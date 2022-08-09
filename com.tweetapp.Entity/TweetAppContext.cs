using com.tweetapp.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Entity
{
    public class TweetAppContext : DbContext
    {
        public TweetAppContext(DbContextOptions<TweetAppContext> options) : base(options)
        {

        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<TweetModel> Tweets { get; set; }
        public DbSet<ReplyModel> Replies { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
    }
}

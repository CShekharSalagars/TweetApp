using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.tweetapp.Entity.Migrations
{
    public partial class RemovedkeyfromTweetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TweetBy",
                table: "Tweet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TweetBy",
                table: "Tweet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

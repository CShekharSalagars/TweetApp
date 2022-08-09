using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.tweetapp.Entity.Migrations
{
    public partial class Addedproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLiked",
                table: "Like",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLiked",
                table: "Like");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perf360.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_Review_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReview_Reviews_ReviewId1",
                table: "UserReview");

            migrationBuilder.DropIndex(
                name: "IX_UserReview_ReviewId1",
                table: "UserReview");

            migrationBuilder.DropColumn(
                name: "ReviewId1",
                table: "UserReview");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "ReviewId1",
                table: "UserReview",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReview_ReviewId1",
                table: "UserReview",
                column: "ReviewId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReview_Reviews_ReviewId1",
                table: "UserReview",
                column: "ReviewId1",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perf360.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Second_Version : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReview",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 COLLATE utf8mb4_zh_0900_as_cs"),
                    ReviewId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ReviewRoleId = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReview", x => new { x.UserId, x.ReviewId, x.ReviewRoleId });
                    table.ForeignKey(
                        name: "FK_UserReview_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReview_ReviewRoles_ReviewRoleId",
                        column: x => x.ReviewRoleId,
                        principalTable: "ReviewRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReview_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 COLLATE utf8mb4_zh_0900_as_cs");

            migrationBuilder.CreateIndex(
                name: "IX_UserReview_ReviewId",
                table: "UserReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReview_ReviewRoleId",
                table: "UserReview",
                column: "ReviewRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReview");
        }
    }
}

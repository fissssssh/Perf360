using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perf360.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Index_For_DeleteAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DeleteAt",
                table: "Reviews",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRecords_DeleteAt",
                table: "ReviewRecords",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDimensions_DeleteAt",
                table: "ReviewDimensions",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeleteAt",
                table: "Departments",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoles_DeleteAt",
                table: "DepartmentRoles",
                column: "DeleteAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_DeleteAt",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_ReviewRecords_DeleteAt",
                table: "ReviewRecords");

            migrationBuilder.DropIndex(
                name: "IX_ReviewDimensions_DeleteAt",
                table: "ReviewDimensions");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DeleteAt",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRoles_DeleteAt",
                table: "DepartmentRoles");
        }
    }
}

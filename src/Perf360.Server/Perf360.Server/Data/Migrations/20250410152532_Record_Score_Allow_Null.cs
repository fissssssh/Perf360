using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perf360.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Record_Score_Allow_Null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Score",
                table: "ReviewRecords",
                type: "float",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Score",
                table: "ReviewRecords",
                type: "float",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);
        }
    }
}

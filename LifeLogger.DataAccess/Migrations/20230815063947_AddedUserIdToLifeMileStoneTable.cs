using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLogger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToLifeMileStoneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LifeMilestones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LifeMilestones_UserId",
                table: "LifeMilestones",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LifeMilestones_AspNetUsers_UserId",
                table: "LifeMilestones",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LifeMilestones_AspNetUsers_UserId",
                table: "LifeMilestones");

            migrationBuilder.DropIndex(
                name: "IX_LifeMilestones_UserId",
                table: "LifeMilestones");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LifeMilestones");
        }
    }
}

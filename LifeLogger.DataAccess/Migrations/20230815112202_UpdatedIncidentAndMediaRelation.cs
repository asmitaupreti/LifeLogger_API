using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLogger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedIncidentAndMediaRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LifeIncidentMedia_Medias_IncidentID",
                table: "LifeIncidentMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LifeIncidentMedia",
                table: "LifeIncidentMedia");

            migrationBuilder.DropIndex(
                name: "IX_LifeIncidentMedia_LifeIncidentsIncidentID",
                table: "LifeIncidentMedia");

            migrationBuilder.DropColumn(
                name: "IncidentID",
                table: "Medias");

            migrationBuilder.RenameColumn(
                name: "IncidentID",
                table: "LifeIncidentMedia",
                newName: "MediaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LifeIncidentMedia",
                table: "LifeIncidentMedia",
                columns: new[] { "LifeIncidentsIncidentID", "MediaID" });

            migrationBuilder.CreateIndex(
                name: "IX_LifeIncidentMedia_MediaID",
                table: "LifeIncidentMedia",
                column: "MediaID");

            migrationBuilder.AddForeignKey(
                name: "FK_LifeIncidentMedia_Medias_MediaID",
                table: "LifeIncidentMedia",
                column: "MediaID",
                principalTable: "Medias",
                principalColumn: "MediaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LifeIncidentMedia_Medias_MediaID",
                table: "LifeIncidentMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LifeIncidentMedia",
                table: "LifeIncidentMedia");

            migrationBuilder.DropIndex(
                name: "IX_LifeIncidentMedia_MediaID",
                table: "LifeIncidentMedia");

            migrationBuilder.RenameColumn(
                name: "MediaID",
                table: "LifeIncidentMedia",
                newName: "IncidentID");

            migrationBuilder.AddColumn<int>(
                name: "IncidentID",
                table: "Medias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LifeIncidentMedia",
                table: "LifeIncidentMedia",
                columns: new[] { "IncidentID", "LifeIncidentsIncidentID" });

            migrationBuilder.CreateIndex(
                name: "IX_LifeIncidentMedia_LifeIncidentsIncidentID",
                table: "LifeIncidentMedia",
                column: "LifeIncidentsIncidentID");

            migrationBuilder.AddForeignKey(
                name: "FK_LifeIncidentMedia_Medias_IncidentID",
                table: "LifeIncidentMedia",
                column: "IncidentID",
                principalTable: "Medias",
                principalColumn: "MediaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

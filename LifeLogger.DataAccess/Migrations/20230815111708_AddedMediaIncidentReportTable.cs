using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLogger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedMediaIncidentReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LifeMilestones_AspNetUsers_UserId",
                table: "LifeMilestones");

            migrationBuilder.DropTable(
                name: "MilestoneTags");

            migrationBuilder.DropIndex(
                name: "IX_LifeMilestones_UserId",
                table: "LifeMilestones");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LifeMilestones");

            migrationBuilder.CreateTable(
                name: "LifeIncidents",
                columns: table => new
                {
                    IncidentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    MilestoneID = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeIncidents", x => x.IncidentID);
                    table.ForeignKey(
                        name: "FK_LifeIncidents_LifeMilestones_MilestoneID",
                        column: x => x.MilestoneID,
                        principalTable: "LifeMilestones",
                        principalColumn: "MilestoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LifeReports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeReports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    MediaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncidentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.MediaID);
                });

            migrationBuilder.CreateTable(
                name: "MilestoneTagMappings",
                columns: table => new
                {
                    MilestoneID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneTagMappings", x => new { x.MilestoneID, x.TagID });
                    table.ForeignKey(
                        name: "FK_MilestoneTagMappings_LifeMilestones_MilestoneID",
                        column: x => x.MilestoneID,
                        principalTable: "LifeMilestones",
                        principalColumn: "MilestoneID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilestoneTagMappings_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilestoneReportMappings",
                columns: table => new
                {
                    MilestoneId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneReportMappings", x => new { x.MilestoneId, x.ReportId });
                    table.ForeignKey(
                        name: "FK_MilestoneReportMappings_LifeMilestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "LifeMilestones",
                        principalColumn: "MilestoneID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilestoneReportMappings_LifeReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "LifeReports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentMediaMappings",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentMediaMappings", x => new { x.IncidentId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_IncidentMediaMappings_LifeIncidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "LifeIncidents",
                        principalColumn: "IncidentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentMediaMappings_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "MediaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LifeIncidentMedia",
                columns: table => new
                {
                    IncidentID = table.Column<int>(type: "int", nullable: false),
                    LifeIncidentsIncidentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeIncidentMedia", x => new { x.IncidentID, x.LifeIncidentsIncidentID });
                    table.ForeignKey(
                        name: "FK_LifeIncidentMedia_LifeIncidents_LifeIncidentsIncidentID",
                        column: x => x.LifeIncidentsIncidentID,
                        principalTable: "LifeIncidents",
                        principalColumn: "IncidentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifeIncidentMedia_Medias_IncidentID",
                        column: x => x.IncidentID,
                        principalTable: "Medias",
                        principalColumn: "MediaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMediaMappings_MediaId",
                table: "IncidentMediaMappings",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_LifeIncidentMedia_LifeIncidentsIncidentID",
                table: "LifeIncidentMedia",
                column: "LifeIncidentsIncidentID");

            migrationBuilder.CreateIndex(
                name: "IX_LifeIncidents_MilestoneID",
                table: "LifeIncidents",
                column: "MilestoneID");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneReportMappings_ReportId",
                table: "MilestoneReportMappings",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneTagMappings_TagID",
                table: "MilestoneTagMappings",
                column: "TagID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentMediaMappings");

            migrationBuilder.DropTable(
                name: "LifeIncidentMedia");

            migrationBuilder.DropTable(
                name: "MilestoneReportMappings");

            migrationBuilder.DropTable(
                name: "MilestoneTagMappings");

            migrationBuilder.DropTable(
                name: "LifeIncidents");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "LifeReports");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LifeMilestones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MilestoneTags",
                columns: table => new
                {
                    MilestoneID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneTags", x => new { x.MilestoneID, x.TagID });
                    table.ForeignKey(
                        name: "FK_MilestoneTags_LifeMilestones_MilestoneID",
                        column: x => x.MilestoneID,
                        principalTable: "LifeMilestones",
                        principalColumn: "MilestoneID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilestoneTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifeMilestones_UserId",
                table: "LifeMilestones",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneTags_TagID",
                table: "MilestoneTags",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_LifeMilestones_AspNetUsers_UserId",
                table: "LifeMilestones",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disc_Store.Data.Migrations
{
    public partial class DiscStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_labels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfPublish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    bandid = table.Column<int>(type: "int", nullable: true),
                    labelid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discs", x => x.id);
                    table.ForeignKey(
                        name: "FK_discs_bands_bandid",
                        column: x => x.bandid,
                        principalTable: "bands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_discs_labels_labelid",
                        column: x => x.labelid,
                        principalTable: "labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "musicians",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roleInGroupid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicians", x => x.id);
                    table.ForeignKey(
                        name: "FK_musicians_roles_roleInGroupid",
                        column: x => x.roleInGroupid,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PivotMusiciansBand",
                columns: table => new
                {
                    bandsid = table.Column<int>(type: "int", nullable: false),
                    musiciansid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PivotMusiciansBand", x => new { x.bandsid, x.musiciansid });
                    table.ForeignKey(
                        name: "FK_PivotMusiciansBand_bands_bandsid",
                        column: x => x.bandsid,
                        principalTable: "bands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PivotMusiciansBand_musicians_musiciansid",
                        column: x => x.musiciansid,
                        principalTable: "musicians",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_discs_bandid",
                table: "discs",
                column: "bandid");

            migrationBuilder.CreateIndex(
                name: "IX_discs_labelid",
                table: "discs",
                column: "labelid");

            migrationBuilder.CreateIndex(
                name: "IX_musicians_roleInGroupid",
                table: "musicians",
                column: "roleInGroupid");

            migrationBuilder.CreateIndex(
                name: "IX_PivotMusiciansBand_musiciansid",
                table: "PivotMusiciansBand",
                column: "musiciansid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discs");

            migrationBuilder.DropTable(
                name: "PivotMusiciansBand");

            migrationBuilder.DropTable(
                name: "labels");

            migrationBuilder.DropTable(
                name: "bands");

            migrationBuilder.DropTable(
                name: "musicians");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}

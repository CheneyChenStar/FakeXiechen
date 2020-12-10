using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiechen.API.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TouristRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Titile = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    OrininalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPresent = table.Column<double>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    DepartureTime = table.Column<DateTime>(nullable: true),
                    Feature = table.Column<string>(nullable: true),
                    Fees = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TouristRoutePictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(maxLength: 100, nullable: true),
                    TouristRouteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristRoutePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristRoutePictures_TouristRoutes_TouristRouteId",
                        column: x => x.TouristRouteId,
                        principalTable: "TouristRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouristRoutePictures_TouristRouteId",
                table: "TouristRoutePictures",
                column: "TouristRouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouristRoutePictures");

            migrationBuilder.DropTable(
                name: "TouristRoutes");
        }
    }
}

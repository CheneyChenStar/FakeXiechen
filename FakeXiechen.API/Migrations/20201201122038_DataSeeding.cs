using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiechen.API.Migrations
{
    public partial class DataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OrininalPrice",
                table: "TouristRoutes",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2");

            migrationBuilder.InsertData(
                table: "TouristRoutes",
                columns: new[] { "Id", "CreateTime", "DepartureTime", "Description", "DiscountPresent", "Feature", "Fees", "Notes", "OrininalPrice", "Titile", "UpdateTime" },
                values: new object[] { new Guid("414be4ff-8d31-4eb6-8a0e-9d3638366286"), new DateTime(2020, 12, 1, 12, 20, 37, 469, DateTimeKind.Utc).AddTicks(4820), null, "this is description", null, null, null, null, 0m, "new Title", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("414be4ff-8d31-4eb6-8a0e-9d3638366286")); 

            migrationBuilder.AlterColumn<decimal>(
                name: "OrininalPrice",
                table: "TouristRoutes",
                type: "decimal(18,2",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }
    }
}

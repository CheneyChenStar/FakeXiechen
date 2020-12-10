using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiechen.API.Migrations
{
    public partial class ShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShaoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShaoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShaoppingCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountPresent = table.Column<double>(nullable: true),
                    TouristRouteId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: true),
                    ShaoppingCartId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItems_ShaoppingCarts_ShaoppingCartId",
                        column: x => x.ShaoppingCartId,
                        principalTable: "ShaoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "ec092c5c-38dc-4943-be5d-cacf87fb9aae");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf3af69a-31ad-4ae1-9cfb-d4cc0f2027db", "AQAAAAEAACcQAAAAEObSuC84jxtyisDkHudsD8dQk3ynG8SDXsEchx/Ur6K7gr3AK4iPBNrZh4GRGvWvuQ==", "6927be27-c52e-49c5-a575-53fc52881e3a" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ShaoppingCartId",
                table: "LineItems",
                column: "ShaoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShaoppingCarts_UserId",
                table: "ShaoppingCarts",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineItems");

            migrationBuilder.DropTable(
                name: "ShaoppingCarts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "3dbe07f0-e338-4715-b0f5-5e4965106793");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a28d9ab5-036c-43f5-8c2e-c596c1617845", "AQAAAAEAACcQAAAAEBuoIbqGAsnf1R+FVhdN1hrU/GPe6kSUdV45shOMJ71OWdxsiraKECuE26c/tlPH+w==", "17326a39-f3fd-4752-8f21-14fe7808bbf2" });
        }
    }
}

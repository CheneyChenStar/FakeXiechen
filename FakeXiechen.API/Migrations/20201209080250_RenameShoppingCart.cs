using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiechen.API.Migrations
{
    public partial class RenameShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_ShaoppingCarts_ShaoppingCartId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_ShaoppingCartId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "ShaoppingCartId",
                table: "LineItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "e2c0fe6c-669f-4435-848b-9aa180b97629");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9cd6a037-ddfb-4ef0-a746-55f022c5cf97", "AQAAAAEAACcQAAAAEJa2oC/y3z0mazs0JEUlX1a38A1TKkgv1suQy2Mv97A5cj0AoH6eVhS5qvMKsntWzg==", "bd88bd42-295b-4c80-b692-e3867bc52dbc" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ShoppingCartId",
                table: "LineItems",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_ShaoppingCarts_ShoppingCartId",
                table: "LineItems",
                column: "ShoppingCartId",
                principalTable: "ShaoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_ShaoppingCarts_ShoppingCartId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_ShoppingCartId",
                table: "LineItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ShaoppingCartId",
                table: "LineItems",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_ShaoppingCarts_ShaoppingCartId",
                table: "LineItems",
                column: "ShaoppingCartId",
                principalTable: "ShaoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiechen.API.Migrations
{
    public partial class ChangeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "1a1e7eaf-ee97-4eaa-b6ec-9165f1bf0bed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed2f8f06-9bee-411d-8b04-e8185d05ef63", "AQAAAAEAACcQAAAAEJnPy5wyVN3aVsVyi0TSseQb3qt5aVJmItd0zM/DNYf1b4nQQiop4Yff5+uEhbYE/w==", "d59c60f9-da27-46be-b1dc-93b6bdcad0e7" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_TouristRouteId",
                table: "LineItems",
                column: "TouristRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_TouristRoutes_TouristRouteId",
                table: "LineItems",
                column: "TouristRouteId",
                principalTable: "TouristRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_TouristRoutes_TouristRouteId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_TouristRouteId",
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
        }
    }
}

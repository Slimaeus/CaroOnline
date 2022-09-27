using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("18d094e9-e04e-4842-8db6-0064c0d232e7"), "8c796724-b72f-43b9-a273-d81019fef819", "Manager Role", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("4ebcc5bb-b4d4-4b26-9d13-861aed7a3d89"), "61f8c6df-5ac9-4aa7-ad8a-1b1843fdceb6", "Admin Role", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("18d094e9-e04e-4842-8db6-0064c0d232e7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4ebcc5bb-b4d4-4b26-9d13-861aed7a3d89"));
        }
    }
}

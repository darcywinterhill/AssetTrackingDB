using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetTrackingDB.Migrations
{
    /// <inheritdoc />
    public partial class AddingSeedingAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "Brand", "Model", "Price", "PurchaseDate", "Type" },
                values: new object[,]
                {
                    { 1, "Lenovo", "IdeaPad", 6290, new DateTime(2021, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop" },
                    { 2, "IPhone", "SE", 6590, new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "MobilePhone" },
                    { 3, "Asus", "Vivobook", 5499, new DateTime(2021, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop" },
                    { 4, "Samsung", "Galaxy S", 8990, new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "MobilePhone" },
                    { 5, "MacBook", "Air", 18975, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop" },
                    { 6, "IPhone", "15", 11990, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "MobilePhone" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

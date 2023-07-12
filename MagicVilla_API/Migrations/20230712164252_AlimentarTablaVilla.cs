using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizaion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa... ", new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1894), new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1881), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Vista a la piscina", new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1900), new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1898), "", 40, "Premium Villa", 4, 220.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregaNumeroVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizaion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 10, 15, 13, 11, 52, 154, DateTimeKind.Local).AddTicks(3739), new DateTime(2023, 10, 15, 13, 11, 52, 154, DateTimeKind.Local).AddTicks(3728) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizaion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 10, 15, 13, 11, 52, 154, DateTimeKind.Local).AddTicks(3742), new DateTime(2023, 10, 15, 13, 11, 52, 154, DateTimeKind.Local).AddTicks(3741) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizaion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1894), new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizaion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1900), new DateTime(2023, 7, 12, 10, 42, 52, 580, DateTimeKind.Local).AddTicks(1898) });
        }
    }
}

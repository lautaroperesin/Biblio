using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FechaDevolucionNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Prestamos",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 22, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2181), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2180) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 25, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2187), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2187) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 20, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2190), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2189) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EjemplarId", "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { 2, new DateTime(2025, 9, 29, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2192), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2191) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EjemplarId", "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { 3, new DateTime(2025, 9, 18, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2194), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2193) });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2092));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2095));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2098));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2102));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Prestamos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 8, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7191), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7190) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 11, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7197), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7197) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 6, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7199), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7199) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EjemplarId", "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { 4, new DateTime(2025, 9, 15, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7201), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7201) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EjemplarId", "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { 5, new DateTime(2025, 9, 4, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7204), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7203) });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7112));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistracion",
                value: new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7118));
        }
    }
}

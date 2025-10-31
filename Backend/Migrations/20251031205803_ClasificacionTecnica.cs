using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class ClasificacionTecnica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CDU",
                table: "Libros",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Libristica",
                table: "Libros",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PalabrasClave",
                table: "Libros",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CDU", "Libristica", "PalabrasClave" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CDU", "Libristica", "PalabrasClave" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CDU", "Libristica", "PalabrasClave" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 11, 7, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9835), new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9834) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 11, 10, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9845), new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9844) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 11, 5, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9847), new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9846) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 11, 14, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9849), new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 11, 3, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9851), new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9850) });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistracion",
                value: new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistracion",
                value: new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistracion",
                value: new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9751));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistracion",
                value: new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9753));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistracion",
                value: new DateTime(2025, 10, 31, 17, 58, 3, 164, DateTimeKind.Local).AddTicks(9754));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CDU",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "Libristica",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "PalabrasClave",
                table: "Libros");

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
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 29, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2192), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2191) });

            migrationBuilder.UpdateData(
                table: "Prestamos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaDevolucion", "FechaPrestamo" },
                values: new object[] { new DateTime(2025, 9, 18, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2194), new DateTime(2025, 9, 15, 16, 13, 35, 90, DateTimeKind.Local).AddTicks(2193) });

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
    }
}

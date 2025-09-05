using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class PrimerMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Editoriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editoriales", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoRol = table.Column<int>(type: "int", nullable: false),
                    FechaRegistracion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Dni = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Domicilio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observacion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sinopsis = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Paginas = table.Column<int>(type: "int", nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false),
                    Portada = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditorialId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libros_Editoriales_EditorialId",
                        column: x => x.EditorialId,
                        principalTable: "Editoriales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioCarreras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CarreraId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCarreras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioCarreras_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCarreras_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ejemplaes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejemplaes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ejemplaes_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LibroAutores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroAutores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibroAutores_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibroAutores_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LibroGeneros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroGeneros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibroGeneros_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibroGeneros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EjemplarId = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamos_Ejemplaes_EjemplarId",
                        column: x => x.EjemplarId,
                        principalTable: "Ejemplaes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prestamos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "Id", "Nombre", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Gabriel García Márquez", false },
                    { 2, "Isabel Allende", false },
                    { 3, "Mario Vargas Llosa", false },
                    { 4, "Jorge Luis Borges", false },
                    { 5, "Pablo Neruda", false },
                    { 6, "Julio Cortázar", false },
                    { 7, "Laura Esquivel", false },
                    { 8, "Carlos Fuentes", false },
                    { 9, "Miguel de Cervantes", false },
                    { 10, "Federico García Lorca", false }
                });

            migrationBuilder.InsertData(
                table: "Carreras",
                columns: new[] { "Id", "Nombre", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Ingeniería", false },
                    { 2, "Literatura", false },
                    { 3, "Matemática", false },
                    { 4, "Historia", false },
                    { 5, "Filosofía", false }
                });

            migrationBuilder.InsertData(
                table: "Editoriales",
                columns: new[] { "Id", "Nombre", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Penguin Random House", false },
                    { 2, "HarperCollins", false },
                    { 3, "Simon & Schuster", false },
                    { 4, "Hachette Livre", false },
                    { 5, "Macmillan Publishers", false },
                    { 6, "Scholastic", false },
                    { 7, "Oxford University Press", false },
                    { 8, "Cambridge University Press", false },
                    { 9, "Wiley", false },
                    { 10, "Springer", false }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Nombre", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Ficción", false },
                    { 2, "No Ficción", false },
                    { 3, "Misterio", false },
                    { 4, "Ciencia Ficción", false },
                    { 5, "Fantasía", false },
                    { 6, "Romance", false },
                    { 7, "Terror", false },
                    { 8, "Aventura", false },
                    { 9, "Historia", false },
                    { 10, "Biografía", false }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Dni", "Domicilio", "Email", "FechaRegistracion", "Nombre", "Observacion", "Password", "Telefono", "TipoRol", "isDeleted" },
                values: new object[,]
                {
                    { 1, "12345678", "Calle Falsa 123", "demo@demo.com", new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7108), "Usuario Demo", "", "1234", "123456789", 0, false },
                    { 2, "87654321", "Calle Verdadera 456", "ana@prueba.com", new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7112), "Ana Prueba", "", "abcd", "987654321", 0, false },
                    { 3, "11223344", "Av. Siempre Viva 742", "carlos@test.com", new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7114), "Carlos Test", "", "pass", "111222333", 0, false },
                    { 4, "55667788", "Calle Real 100", "lucia@ejemplo.com", new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7116), "Lucía Ejemplo", "", "lucia", "444555666", 0, false },
                    { 5, "99887766", "Calle Nueva 321", "pedro@alumno.com", new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7118), "Pedro Alumno", "", "pedro", "777888999", 0, false }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "Id", "AnioPublicacion", "Descripcion", "EditorialId", "Paginas", "Portada", "Sinopsis", "Titulo", "isDeleted" },
                values: new object[,]
                {
                    { 1, 1967, "Una novela emblemática del realismo mágico.", 1, 417, "https://example.com/cien_anos_de_soledad.jpg", "La historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.", "Cien Años de Soledad", false },
                    { 2, 1982, "Una saga familiar con elementos sobrenaturales.", 2, 448, "https://example.com/la_casa_de_los_espiritus.jpg", "La historia de la familia Trueba y sus experiencias a lo largo de varias generaciones en Chile.", "La Casa de los Espíritus", false },
                    { 3, 1605, "Una novela clásica de la literatura española.", 3, 863, "https://example.com/don_quijote.jpg", "Las aventuras del ingenioso hidalgo Don Quijote y su fiel escudero Sancho Panza.", "Don Quijote de la Mancha", false }
                });

            migrationBuilder.InsertData(
                table: "UsuarioCarreras",
                columns: new[] { "Id", "CarreraId", "UsuarioId", "isDeleted" },
                values: new object[,]
                {
                    { 1, 1, 1, false },
                    { 2, 2, 2, false },
                    { 3, 3, 3, false },
                    { 4, 4, 4, false },
                    { 5, 5, 5, false }
                });

            migrationBuilder.InsertData(
                table: "Ejemplaes",
                columns: new[] { "Id", "Disponible", "Estado", "LibroId", "isDeleted" },
                values: new object[,]
                {
                    { 1, true, 1, 1, false },
                    { 2, false, 1, 2, false },
                    { 3, true, 3, 3, false },
                    { 4, true, 1, 3, false },
                    { 5, false, 2, 2, false }
                });

            migrationBuilder.InsertData(
                table: "LibroAutores",
                columns: new[] { "Id", "AutorId", "LibroId", "isDeleted" },
                values: new object[,]
                {
                    { 1, 1, 1, false },
                    { 2, 2, 2, false },
                    { 3, 3, 3, false },
                    { 4, 4, 3, false },
                    { 5, 2, 2, false }
                });

            migrationBuilder.InsertData(
                table: "LibroGeneros",
                columns: new[] { "Id", "GeneroId", "LibroId", "isDeleted" },
                values: new object[,]
                {
                    { 1, 1, 1, false },
                    { 2, 2, 2, false },
                    { 3, 1, 3, false },
                    { 4, 5, 2, false },
                    { 5, 3, 3, false }
                });

            migrationBuilder.InsertData(
                table: "Prestamos",
                columns: new[] { "Id", "EjemplarId", "FechaDevolucion", "FechaPrestamo", "UsuarioId", "isDeleted" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 9, 8, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7191), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7190), 1, false },
                    { 2, 2, new DateTime(2025, 9, 11, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7197), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7197), 2, false },
                    { 3, 3, new DateTime(2025, 9, 6, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7199), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7199), 3, false },
                    { 4, 4, new DateTime(2025, 9, 15, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7201), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7201), 4, false },
                    { 5, 5, new DateTime(2025, 9, 4, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7204), new DateTime(2025, 9, 1, 16, 24, 38, 724, DateTimeKind.Local).AddTicks(7203), 5, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ejemplaes_LibroId",
                table: "Ejemplaes",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroAutores_AutorId",
                table: "LibroAutores",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroAutores_LibroId",
                table: "LibroAutores",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroGeneros_GeneroId",
                table: "LibroGeneros",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroGeneros_LibroId",
                table: "LibroGeneros",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_EditorialId",
                table: "Libros",
                column: "EditorialId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_EjemplarId",
                table: "Prestamos",
                column: "EjemplarId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_UsuarioId",
                table: "Prestamos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCarreras_CarreraId",
                table: "UsuarioCarreras",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCarreras_UsuarioId",
                table: "UsuarioCarreras",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibroAutores");

            migrationBuilder.DropTable(
                name: "LibroGeneros");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "UsuarioCarreras");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Ejemplaes");

            migrationBuilder.DropTable(
                name: "Carreras");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Editoriales");
        }
    }
}

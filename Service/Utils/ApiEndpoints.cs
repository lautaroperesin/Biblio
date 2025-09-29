using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public static class ApiEndpoints
    {
        public static string Libro { get; set; } = "libros";
        public static string Editorial { get; set; } = "editoriales";
        public static string Autor { get; set; } = "autores";
        public static string Ejemplar { get; set; } = "ejemplares";
        public static string Carrera { get; set; } = "carreras";
        public static string Genero { get; set; } = "generos";
        public static string Prestamo { get; set; } = "prestamos";
        public static string Usuario { get; set; } = "usuarios";
        public static string LibroAutor { get; set; } = "librosautores";
        public static string LibroGenero { get; set; } = "librosgeneros";
        public static string UsuarioCarrera { get; set; } = "usuarioscarreras";
        public static string Gemini { get; set; } = "gemini";
        public static string Login { get; set; } = "auth";
        public static string UsuarioInstitutoApp { get; set; } = "apiusuarios";

        public static string GetEndpoint(string name)
        {
            return name switch
            {
                nameof(Libro) => Libro,
                nameof(Editorial) => Editorial,
                nameof(Autor) => Autor,
                nameof(Ejemplar) => Ejemplar,
                nameof(Carrera) => Carrera,
                nameof(Genero) => Genero,
                nameof(Prestamo) => Prestamo,
                nameof(Usuario) => Usuario,
                nameof(LibroAutor) => LibroAutor,
                nameof(LibroGenero) => LibroGenero,
                nameof(UsuarioCarrera) => UsuarioCarrera,
                nameof(Gemini) => Gemini,
                nameof(Login) => Login,
                nameof(UsuarioInstitutoApp) => UsuarioInstitutoApp,
                _ => throw new ArgumentException($"Endpoint '{name}' no está definido.")
            };
        }
    }
}

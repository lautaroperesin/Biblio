    using Microsoft.EntityFrameworkCore;
using Service.Enums;
using Service.Models;

namespace Backend.DataContext
{
    public class BiblioContext : DbContext
    {
        public BiblioContext() { }
        public BiblioContext(DbContextOptions<BiblioContext> options) : base(options)
        {
        }

        public virtual DbSet<Libro> Libros { get; set; }
        public virtual DbSet<Autor> Autores { get; set; }
        public virtual DbSet<Editorial> Editoriales { get; set; }
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<LibroAutor> LibroAutores { get; set; }
        public virtual DbSet<LibroGenero> LibroGeneros { get; set; }
        public virtual DbSet<Ejemplar> Ejemplaes { get; set; }
        public virtual DbSet<Prestamo> Prestamos { get; set; }
        public virtual DbSet<Carrera> Carreras { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioCarrera> UsuarioCarreras { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Datos semilla
            // Autores
            modelBuilder.Entity<Autor>().HasData(
                new Autor { Id = 1, Nombre = "Gabriel García Márquez" },
                new Autor { Id = 2, Nombre = "Isabel Allende" },
                new Autor { Id = 3, Nombre = "Mario Vargas Llosa" },
                new Autor { Id = 4, Nombre = "Jorge Luis Borges" },
                new Autor { Id = 5, Nombre = "Pablo Neruda" },
                new Autor { Id = 6, Nombre = "Julio Cortázar" },
                new Autor { Id = 7, Nombre = "Laura Esquivel" },
                new Autor { Id = 8, Nombre = "Carlos Fuentes" },
                new Autor { Id = 9, Nombre = "Miguel de Cervantes" },
                new Autor { Id = 10, Nombre = "Federico García Lorca" }
            );

            // Datos semilla de generos
            modelBuilder.Entity<Genero>().HasData(
                new Genero { Id = 1, Nombre = "Ficción" },
                new Genero { Id = 2, Nombre = "No Ficción" },
                new Genero { Id = 3, Nombre = "Misterio" },
                new Genero { Id = 4, Nombre = "Ciencia Ficción" },
                new Genero { Id = 5, Nombre = "Fantasía" },
                new Genero { Id = 6, Nombre = "Romance" },
                new Genero { Id = 7, Nombre = "Terror" },
                new Genero { Id = 8, Nombre = "Aventura" },
                new Genero { Id = 9, Nombre = "Historia" },
                new Genero { Id = 10, Nombre = "Biografía" }
                );

            // Datos semilla de editoriales
            modelBuilder.Entity<Editorial>().HasData(
                new Editorial { Id = 1, Nombre = "Penguin Random House" },
                new Editorial { Id = 2, Nombre = "HarperCollins" },
                new Editorial { Id = 3, Nombre = "Simon & Schuster" },
                new Editorial { Id = 4, Nombre = "Hachette Livre" },
                new Editorial { Id = 5, Nombre = "Macmillan Publishers" },
                new Editorial { Id = 6, Nombre = "Scholastic" },
                new Editorial { Id = 7, Nombre = "Oxford University Press" },
                new Editorial { Id = 8, Nombre = "Cambridge University Press" },
                new Editorial { Id = 9, Nombre = "Wiley" },
                new Editorial { Id = 10, Nombre = "Springer" }
                );

            // Datos semilla de libros
            modelBuilder.Entity<Libro>().HasData(
                new Libro
                {
                    Id = 1,
                    Titulo = "Cien Años de Soledad",
                    Descripcion = "Una novela emblemática del realismo mágico.",
                    Sinopsis = "La historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                    Paginas = 417,
                    AnioPublicacion = 1967,
                    Portada = "https://example.com/cien_anos_de_soledad.jpg",
                    EditorialId = 1
                },
                new Libro
                {
                    Id = 2,
                    Titulo = "La Casa de los Espíritus",
                    Descripcion = "Una saga familiar con elementos sobrenaturales.",
                    Sinopsis = "La historia de la familia Trueba y sus experiencias a lo largo de varias generaciones en Chile.",
                    Paginas = 448,
                    AnioPublicacion = 1982,
                    Portada = "https://example.com/la_casa_de_los_espiritus.jpg",
                    EditorialId = 2
                },
                new Libro
                {
                    Id = 3,
                    Titulo = "Don Quijote de la Mancha",
                    Descripcion = "Una novela clásica de la literatura española.",
                    Sinopsis = "Las aventuras del ingenioso hidalgo Don Quijote y su fiel escudero Sancho Panza.",
                    Paginas = 863,
                    AnioPublicacion = 1605,
                    Portada = "https://example.com/don_quijote.jpg",
                    EditorialId = 3
                }
            );

            // LibroAutores
            modelBuilder.Entity<LibroAutor>().HasData(
                new LibroAutor { Id = 1, LibroId = 1, AutorId = 1, isDeleted = false },
                new LibroAutor { Id = 2, LibroId = 2, AutorId = 2, isDeleted = false },
                new LibroAutor { Id = 3, LibroId = 3, AutorId = 3, isDeleted = false },
                new LibroAutor { Id = 4, LibroId = 3, AutorId = 4, isDeleted = false },
                new LibroAutor { Id = 5, LibroId = 2, AutorId = 2, isDeleted = false }
            );

            // LibroGeneros
            modelBuilder.Entity<LibroGenero>().HasData(
                new LibroGenero { Id = 1, LibroId = 1, GeneroId = 1, isDeleted = false },
                new LibroGenero { Id = 2, LibroId = 2, GeneroId = 2, isDeleted = false },
                new LibroGenero { Id = 3, LibroId = 3, GeneroId = 1, isDeleted = false },
                new LibroGenero { Id = 4, LibroId = 2, GeneroId = 5, isDeleted = false },
                new LibroGenero { Id = 5, LibroId = 3, GeneroId = 3, isDeleted = false }
            );

            // Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Usuario Demo", Email = "demo@demo.com", Password = "1234", TipoRol = TipoRolEnum.Alumno, FechaRegistracion = DateTime.Now, Dni = "12345678", Domicilio = "Calle Falsa 123", Telefono = "123456789", Observacion = "", isDeleted = false },
                new Usuario { Id = 2, Nombre = "Ana Prueba", Email = "ana@prueba.com", Password = "abcd", TipoRol = TipoRolEnum.Alumno, FechaRegistracion = DateTime.Now, Dni = "87654321", Domicilio = "Calle Verdadera 456", Telefono = "987654321", Observacion = "", isDeleted = false },
                new Usuario { Id = 3, Nombre = "Carlos Test", Email = "carlos@test.com", Password = "pass", TipoRol = TipoRolEnum.Alumno, FechaRegistracion = DateTime.Now, Dni = "11223344", Domicilio = "Av. Siempre Viva 742", Telefono = "111222333", Observacion = "", isDeleted = false },
                new Usuario { Id = 4, Nombre = "Lucía Ejemplo", Email = "lucia@ejemplo.com", Password = "lucia", TipoRol = TipoRolEnum.Alumno, FechaRegistracion = DateTime.Now, Dni = "55667788", Domicilio = "Calle Real 100", Telefono = "444555666", Observacion = "", isDeleted = false },
                new Usuario { Id = 5, Nombre = "Pedro Alumno", Email = "pedro@alumno.com", Password = "pedro", TipoRol = TipoRolEnum.Alumno, FechaRegistracion = DateTime.Now, Dni = "99887766", Domicilio = "Calle Nueva 321", Telefono = "777888999", Observacion = "", isDeleted = false }
            );

            // Carreras
            modelBuilder.Entity<Carrera>().HasData(
                new Carrera { Id = 1, Nombre = "Ingeniería", isDeleted = false },
                new Carrera { Id = 2, Nombre = "Literatura", isDeleted = false },
                new Carrera { Id = 3, Nombre = "Matemática", isDeleted = false },
                new Carrera { Id = 4, Nombre = "Historia", isDeleted = false },
                new Carrera { Id = 5, Nombre = "Filosofía", isDeleted = false }
            );

            // UsuarioCarreras
            modelBuilder.Entity<UsuarioCarrera>().HasData(
                new UsuarioCarrera { Id = 1, UsuarioId = 1, CarreraId = 1, isDeleted = false },
                new UsuarioCarrera { Id = 2, UsuarioId = 2, CarreraId = 2, isDeleted = false },
                new UsuarioCarrera { Id = 3, UsuarioId = 3, CarreraId = 3, isDeleted = false },
                new UsuarioCarrera { Id = 4, UsuarioId = 4, CarreraId = 4, isDeleted = false },
                new UsuarioCarrera { Id = 5, UsuarioId = 5, CarreraId = 5, isDeleted = false }
            );

            // Ejemplares
            modelBuilder.Entity<Ejemplar>().HasData(
                new Ejemplar { Id = 1, LibroId = 1, Disponible = true, Estado = EstadoEjemplarEnum.MuyBueno, isDeleted = false },
                new Ejemplar { Id = 2, LibroId = 2, Disponible = false, Estado = EstadoEjemplarEnum.MuyBueno, isDeleted = false },
                new Ejemplar { Id = 3, LibroId = 3, Disponible = true, Estado = EstadoEjemplarEnum.Regular, isDeleted = false },
                new Ejemplar { Id = 4, LibroId = 3, Disponible = true, Estado = EstadoEjemplarEnum.MuyBueno, isDeleted = false },
                new Ejemplar { Id = 5, LibroId = 2, Disponible = false, Estado = EstadoEjemplarEnum.Bueno, isDeleted = false }
            );

            // Prestamos
            modelBuilder.Entity<Prestamo>().HasData(
                new Prestamo { Id = 1, UsuarioId = 1, EjemplarId = 1, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now.AddDays(7), isDeleted = false },
                new Prestamo { Id = 2, UsuarioId = 2, EjemplarId = 2, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now.AddDays(10), isDeleted = false },
                new Prestamo { Id = 3, UsuarioId = 3, EjemplarId = 3, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now.AddDays(5), isDeleted = false },
                new Prestamo { Id = 4, UsuarioId = 4, EjemplarId = 2, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now.AddDays(14), isDeleted = false },
                new Prestamo { Id = 5, UsuarioId = 5, EjemplarId = 3, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now.AddDays(3), isDeleted = false }
            );


            #endregion

            // Query filters para no traer los eliminados
            modelBuilder.Entity<Libro>().HasQueryFilter(l => !l.isDeleted);
            modelBuilder.Entity<Autor>().HasQueryFilter(a => !a.isDeleted);
            modelBuilder.Entity<Genero>().HasQueryFilter(g => !g.isDeleted);
            modelBuilder.Entity<Editorial>().HasQueryFilter(e => !e.isDeleted);
            modelBuilder.Entity<Ejemplar>().HasQueryFilter(ej => !ej.isDeleted);
            modelBuilder.Entity<Carrera>().HasQueryFilter(c => !c.isDeleted);
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => !u.isDeleted);
            modelBuilder.Entity<UsuarioCarrera>().HasQueryFilter(uc => !uc.isDeleted);
            modelBuilder.Entity<Prestamo>().HasQueryFilter(p => !p.isDeleted);
            modelBuilder.Entity<LibroAutor>().HasQueryFilter(la => !la.isDeleted);
            modelBuilder.Entity<LibroGenero>().HasQueryFilter(lg => !lg.isDeleted);
        }
    }
}

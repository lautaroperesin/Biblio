using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataContext;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibrosController : ControllerBase
    {
        private readonly BiblioContext _context;

        public LibrosController(BiblioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros([FromQuery] string filtro = "")
        {
            return await _context.Libros
                .Include(l => l.Editorial)
                .Include(l => l.LibrosAutores).ThenInclude(la => la.Autor)
                .Include(l => l.LibrosGeneros).ThenInclude(lg => lg.Genero)
                .AsNoTracking()
                .Where(l => l.Titulo.Contains(filtro)).ToListAsync();
        }

        [HttpPost("withfilter")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosWithFilter(FilterLibroDTO filter)
        {
            // Construir la query base con todas las relaciones necesarias
            var query = _context.Libros
                .Include(l => l.Editorial)
                .Include(l => l.LibrosAutores).ThenInclude(la => la.Autor)
                .Include(l => l.LibrosGeneros).ThenInclude(lg => lg.Genero)
                .AsNoTracking();

            // Si no hay texto de búsqueda, retornar todos los libros
            if (string.IsNullOrWhiteSpace(filter.SearchText))
            {
                return await query.ToListAsync();
            }

            var searchTerm = filter.SearchText.ToLower();

            // Verificar si al menos un filtro está activado
            bool anyFilterActive = filter.ForTitulo || filter.ForAutor || filter.ForEditorial || filter.ForGenero;

            // Si no hay filtros activados, buscar en todos los campos por defecto
            if (!anyFilterActive)
            {
                query = query.Where(l =>
                    l.Titulo.ToLower().Contains(searchTerm) ||
                    (l.Editorial != null && l.Editorial.Nombre.ToLower().Contains(searchTerm)) ||
                    l.LibrosAutores.Any(la => la.Autor != null && la.Autor.Nombre.ToLower().Contains(searchTerm)) ||
                    l.LibrosGeneros.Any(lg => lg.Genero != null && lg.Genero.Nombre.ToLower().Contains(searchTerm))
                );
            }
            else
            {
                // Aplicar filtros específicos según los booleanos activados
                query = query.Where(l =>
                    (filter.ForTitulo && l.Titulo.ToLower().Contains(searchTerm)) ||
                    (filter.ForEditorial && l.Editorial != null && l.Editorial.Nombre.ToLower().Contains(searchTerm)) ||
                    (filter.ForAutor && l.LibrosAutores.Any(la => la.Autor != null && la.Autor.Nombre.ToLower().Contains(searchTerm))) ||
                    (filter.ForGenero && l.LibrosGeneros.Any(lg => lg.Genero != null && lg.Genero.Nombre.ToLower().Contains(searchTerm)))
                );
            }

            return await query.ToListAsync();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetDeletedsLibros([FromQuery] string filtro = "")
        {
            return await _context.Libros.AsNoTracking().IgnoreQueryFilters().Where(l => l.isDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.Include(l => l.Editorial)
                .Include(l => l.LibrosAutores).ThenInclude(la => la.Autor)
                .Include(l => l.LibrosGeneros).ThenInclude(lg => lg.Genero)
                .AsNoTracking().FirstOrDefaultAsync(l => l.Id.Equals(id));

            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
            var libroAnterior = await _context.Libros
                                .Include(l => l.Editorial)
                                .Include(l => l.LibrosAutores).ThenInclude(la => la.Autor)
                                .Include(l => l.LibrosGeneros).ThenInclude(lg => lg.Genero)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(l => l.Id.Equals(id));
            if (libroAnterior == null)
            {
                return NotFound();
            }


            //buscamos autores y generos nuevos
            var autoresNuevos = libro.LibrosAutores.Where(la => !libroAnterior.LibrosAutores.Any(aa => aa.AutorId == la.AutorId)).ToList();
            var generosNuevos = libro.LibrosGeneros.Where(lg => !libroAnterior.LibrosGeneros.Any(ga => ga.GeneroId == lg.GeneroId)).ToList();

            //buscamos autores y géneros eliminados
            var autoresEliminados = libroAnterior.LibrosAutores.Where(aa => !libro.LibrosAutores.Any(la => la.AutorId == aa.AutorId)).ToList();
            var generosEliminados = libroAnterior.LibrosGeneros.Where(ga => !libro.LibrosGeneros.Any(lg => lg.GeneroId == ga.GeneroId)).ToList();

            //actualizamos las listas
            foreach (var autor in autoresNuevos)
            {
                _context.LibroAutores.Add(new LibroAutor { LibroId = libro.Id, AutorId = autor.AutorId });
            }
            foreach (var genero in generosNuevos)
            {
                _context.LibroGeneros.Add(new LibroGenero { LibroId = libro.Id, GeneroId = genero.GeneroId });
            }
            //eliminamos los autores y géneros eliminados
            foreach (var autor in autoresEliminados)
            {
                var libroAutor = await _context.LibroAutores.FirstOrDefaultAsync(la => la.LibroId == libro.Id && la.AutorId == autor.AutorId);
                if (libroAutor != null)
                {
                    _context.LibroAutores.Remove(libroAutor);
                }
            }
            foreach (var genero in generosEliminados)
            {
                var libroGenero = await _context.LibroGeneros.FirstOrDefaultAsync(lg => lg.LibroId == libro.Id && lg.GeneroId == genero.GeneroId);
                if (libroGenero != null)
                {
                    _context.LibroGeneros.Remove(libroGenero);
                }
            }


            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            libro.isDeleted = true;
            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreLibro(int id)
        {
            var libro = await _context.Libros.IgnoreQueryFilters().FirstOrDefaultAsync(l => l.Id.Equals(id));
            if (libro == null)
            {
                return NotFound();
            }
            libro.isDeleted = false;
            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}

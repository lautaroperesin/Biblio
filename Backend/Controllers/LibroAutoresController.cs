using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.ExtentionMethods;
using Service.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibroAutoresController : ControllerBase
    {
        private readonly BiblioContext _context;

        public LibroAutoresController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/LibroAutores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroAutor>>> GetLibroAutores([FromQuery] string filtro = "")
        {
            var query = _context.LibroAutores.AsNoTracking()
                .Include(la => la.Libro)
                .Include(la => la.Autor)
                .Where(la => la.Libro.Titulo.ToUpper().Contains(filtro.ToUpper()) || la.Autor.Nombre.ToUpper().Contains(filtro.ToUpper())
            );
            return await query.ToListAsync();
        }

        // GET: api/LibroAutores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroAutor>> GetLibroAutor(int id)
        {
            var libroAutor = await _context.LibroAutores.AsNoTracking().FirstOrDefaultAsync(la => la.Id == id);
            if (libroAutor == null)
            {
                return NotFound();
            }
            return libroAutor;
        }

        // PUT: api/LibroAutores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibroAutor(int id, LibroAutor libroAutor)
        {
            _context.TryAttach(libroAutor?.Libro);
            _context.TryAttach(libroAutor?.Autor);
            _context.TryAttach(libroAutor?.Libro?.Editorial);

            if (id != libroAutor.Id)
            {
                return BadRequest();
            }
            _context.Entry(libroAutor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroAutorExists(id))
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

        // POST: api/LibroAutores
        [HttpPost]
        public async Task<ActionResult<LibroAutor>> PostLibroAutor(LibroAutor libroAutor)
        {
            _context.TryAttach(libroAutor?.Libro);
            _context.TryAttach(libroAutor?.Autor);
            _context.TryAttach(libroAutor?.Libro?.Editorial);
            _context.LibroAutores.Add(libroAutor);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLibroAutor", new { id = libroAutor.Id }, libroAutor);
        }

        // DELETE: api/LibroAutores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibroAutor(int id)
        {
            var libroAutor = await _context.LibroAutores.FindAsync(id);
            if (libroAutor == null)
            {
                return NotFound();
            }
            libroAutor.isDeleted = true;
            _context.LibroAutores.Update(libroAutor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreLibroAutor(int id)
        {
            var libroAutor = await _context.LibroAutores.IgnoreQueryFilters().FirstOrDefaultAsync(la => la.Id == id);
            if (libroAutor == null)
            {
                return NotFound();
            }
            libroAutor.isDeleted = false;
            _context.LibroAutores.Update(libroAutor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LibroAutorExists(int id)
        {
            return _context.LibroAutores.Any(e => e.Id == id);
        }
    }
}

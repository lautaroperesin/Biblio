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
    public class LibroGenerosController : ControllerBase
    {
        private readonly BiblioContext _context;

        public LibroGenerosController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/LibroGeneros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroGenero>>> GetLibroGeneros([FromQuery] int? libroId = null, [FromQuery] int? generoId = null)
        {
            var query = _context.LibroGeneros.AsNoTracking();
            if (libroId.HasValue)
                query = query.Where(lg => lg.LibroId == libroId);
            if (generoId.HasValue)
                query = query.Where(lg => lg.GeneroId == generoId);
            return await query.ToListAsync();
        }

        // GET: api/LibroGeneros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroGenero>> GetLibroGenero(int id)
        {
            var libroGenero = await _context.LibroGeneros.AsNoTracking().FirstOrDefaultAsync(lg => lg.Id == id);
            if (libroGenero == null)
            {
                return NotFound();
            }
            return libroGenero;
        }

        // PUT: api/LibroGeneros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibroGenero(int id, LibroGenero libroGenero)
        {
            _context.TryAttach(libroGenero?.Libro);
            _context.TryAttach(libroGenero?.Genero);
            if (id != libroGenero.Id)
            {
                return BadRequest();
            }
            _context.Entry(libroGenero).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroGeneroExists(id))
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

        // POST: api/LibroGeneros
        [HttpPost]
        public async Task<ActionResult<LibroGenero>> PostLibroGenero(LibroGenero libroGenero)
        {
            _context.TryAttach(libroGenero?.Libro);
            _context.TryAttach(libroGenero?.Genero);
            _context.LibroGeneros.Add(libroGenero);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLibroGenero", new { id = libroGenero.Id }, libroGenero);
        }

        // DELETE: api/LibroGeneros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibroGenero(int id)
        {
            var libroGenero = await _context.LibroGeneros.FindAsync(id);
            if (libroGenero == null)
            {
                return NotFound();
            }
            libroGenero.isDeleted = true;
            _context.LibroGeneros.Update(libroGenero);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreLibroGenero(int id)
        {
            var libroGenero = await _context.LibroGeneros.IgnoreQueryFilters().FirstOrDefaultAsync(lg => lg.Id == id);
            if (libroGenero == null)
            {
                return NotFound();
            }
            libroGenero.isDeleted = false;
            _context.LibroGeneros.Update(libroGenero);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LibroGeneroExists(int id)
        {
            return _context.LibroGeneros.Any(e => e.Id == id);
        }
    }
}

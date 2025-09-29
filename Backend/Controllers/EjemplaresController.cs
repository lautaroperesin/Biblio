using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.ExtensionMethods;
using Service.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EjemplaresController : ControllerBase
    {
        private readonly BiblioContext _context;

        public EjemplaresController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/Ejemplares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ejemplar>>> GetEjemplares([FromQuery] int? libroId = null, [FromQuery] bool? disponible = null)
        {
            var query = _context.Ejemplaes.AsNoTracking();
            if (libroId.HasValue)
                query = query.Where(e => e.LibroId == libroId);
            if (disponible.HasValue)
                query = query.Where(e => e.Disponible == disponible);
            return await query.ToListAsync();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Ejemplar>>> GetDeletedsEjemplares()
        {
            return await _context.Ejemplaes.AsNoTracking().IgnoreQueryFilters().Where(e => e.isDeleted).ToListAsync();
        }

        // GET: api/Ejemplares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ejemplar>> GetEjemplar(int id)
        {
            var ejemplar = await _context.Ejemplaes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (ejemplar == null)
            {
                return NotFound();
            }
            return ejemplar;
        }

        // PUT: api/Ejemplares/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEjemplar(int id, Ejemplar ejemplar)
        {
            _context.TryAttach(ejemplar?.Libro);
            if (id != ejemplar.Id)
            {
                return BadRequest();
            }
            _context.Entry(ejemplar).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EjemplarExists(id))
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

        // POST: api/Ejemplares
        [HttpPost]
        public async Task<ActionResult<Ejemplar>> PostEjemplar(Ejemplar ejemplar)
        {
            _context.TryAttach(ejemplar?.Libro);
            _context.Ejemplaes.Add(ejemplar);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEjemplar", new { id = ejemplar.Id }, ejemplar);
        }

        // DELETE: api/Ejemplares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEjemplar(int id)
        {
            var ejemplar = await _context.Ejemplaes.FindAsync(id);
            if (ejemplar == null)
            {
                return NotFound();
            }
            ejemplar.isDeleted = true;
            _context.Ejemplaes.Update(ejemplar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreEjemplar(int id)
        {
            var ejemplar = await _context.Ejemplaes.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (ejemplar == null)
            {
                return NotFound();
            }
            ejemplar.isDeleted = false;
            _context.Ejemplaes.Update(ejemplar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EjemplarExists(int id)
        {
            return _context.Ejemplaes.Any(e => e.Id == id);
        }
    }
}

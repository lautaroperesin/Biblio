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
    public class PrestamosController : ControllerBase
    {
        private readonly BiblioContext _context;

        public PrestamosController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos([FromQuery] int? usuarioId = null, [FromQuery] int? ejemplarId = null, [FromQuery] bool? activos = null)
        {
            var query = _context.Prestamos.AsNoTracking();
            if (usuarioId.HasValue)
                query = query.Where(p => p.UsuarioId == usuarioId);
            if (ejemplarId.HasValue)
                query = query.Where(p => p.EjemplarId == ejemplarId);
            if (activos.HasValue && activos.Value)
                query = query.Where(p => p.FechaDevolucion > DateTime.Now);
            return await query.ToListAsync();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetDeletedsPrestamos()
        {
            return await _context.Prestamos.AsNoTracking().IgnoreQueryFilters().Where(p => p.isDeleted).ToListAsync();
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return prestamo;
        }

        // PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            _context.TryAttach(prestamo?.Usuario);
            _context.TryAttach(prestamo?.Ejemplar);
            if (id != prestamo.Id)
            {
                return BadRequest();
            }
            _context.Entry(prestamo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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

        // POST: api/Prestamos
        [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
        {
            _context.TryAttach(prestamo?.Usuario);
            _context.TryAttach(prestamo?.Ejemplar);
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            prestamo.isDeleted = true;
            _context.Prestamos.Update(prestamo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestorePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }
            prestamo.isDeleted = false;
            _context.Prestamos.Update(prestamo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarrerasController : ControllerBase
    {
        private readonly BiblioContext _context;

        public CarrerasController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/Carreras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreras([FromQuery] string nombre = "")
        {
            return await _context.Carreras.AsNoTracking().Where(c => c.Nombre.Contains(nombre)).ToListAsync();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetDeletedsCarreras()
        {
            return await _context.Carreras.AsNoTracking().IgnoreQueryFilters().Where(c => c.isDeleted).ToListAsync();
        }

        // GET: api/Carreras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrera>> GetCarrera(int id)
        {
            var carrera = await _context.Carreras.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }
            return carrera;
        }

        // PUT: api/Carreras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrera(int id, Carrera carrera)
        {
            // No entidades relacionadas a attachear
            if (id != carrera.Id)
            {
                return BadRequest();
            }
            _context.Entry(carrera).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
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

        // POST: api/Carreras
        [HttpPost]
        public async Task<ActionResult<Carrera>> PostCarrera(Carrera carrera)
        {
            // No entidades relacionadas a attachear
            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCarrera", new { id = carrera.Id }, carrera);
        }

        // DELETE: api/Carreras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrera(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }
            carrera.isDeleted = true;
            _context.Carreras.Update(carrera);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreCarrera(int id)
        {
            var carrera = await _context.Carreras.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }
            carrera.isDeleted = false;
            _context.Carreras.Update(carrera);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CarreraExists(int id)
        {
            return _context.Carreras.Any(e => e.Id == id);
        }
    }
}

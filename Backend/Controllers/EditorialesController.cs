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
    public class EditorialesController : ControllerBase
    {
        private readonly BiblioContext _context;

        public EditorialesController(BiblioContext context)
        {
            _context = context;
        }

        // GET: api/Editoriales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditoriales([FromQuery] string nombre = "")
        {
            return await _context.Editoriales.AsNoTracking().Where(e => e.Nombre.Contains(nombre)).ToListAsync();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetDeletedsEditoriales()
        {
            return await _context.Editoriales.AsNoTracking().IgnoreQueryFilters().Where(e => e.isDeleted).ToListAsync();
        }

        // GET: api/Editoriales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
            var editorial = await _context.Editoriales.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (editorial == null)
            {
                return NotFound();
            }
            return editorial;
        }

        // PUT: api/Editoriales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            // No entidades relacionadas a attachear
            if (id != editorial.Id)
            {
                return BadRequest();
            }
            _context.Entry(editorial).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        // POST: api/Editoriales
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
            // No entidades relacionadas a attachear
            _context.Editoriales.Add(editorial);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEditorial", new { id = editorial.Id }, editorial);
        }

        // DELETE: api/Editoriales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            var editorial = await _context.Editoriales.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }
            editorial.isDeleted = true;
            _context.Editoriales.Update(editorial);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreEditorial(int id)
        {
            var editorial = await _context.Editoriales.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (editorial == null)
            {
                return NotFound();
            }
            editorial.isDeleted = false;
            _context.Editoriales.Update(editorial);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return _context.Editoriales.Any(e => e.Id == id);
        }
    }
}

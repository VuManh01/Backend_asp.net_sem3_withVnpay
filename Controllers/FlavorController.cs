using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project3api_be.Data;
using project3api_be.Models;

namespace project3api_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlavorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlavorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Flavor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flavor>>> GetFlavors()
        {
            return await _context.Flavors.ToListAsync();
        }

        // GET: api/Flavor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flavor>> GetFlavor(int id)
        {
            var flavor = await _context.Flavors.FindAsync(id);

            if (flavor == null)
            {
                return NotFound();
            }

            return flavor;
        }

        // POST: api/Flavor
        [HttpPost]
        public async Task<ActionResult<Flavor>> CreateFlavor(Flavor flavor)
        {
            _context.Flavors.Add(flavor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlavor), new { id = flavor.FlavorId }, flavor);
        }

        // PUT: api/Flavor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlavor(int id, Flavor flavor)
        {
            if (id != flavor.FlavorId)
            {
                return BadRequest();
            }

            _context.Entry(flavor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlavorExists(id))
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

        // DELETE: api/Flavor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlavor(int id)
        {
            var flavor = await _context.Flavors.FindAsync(id);
            if (flavor == null)
            {
                return NotFound();
            }

            _context.Flavors.Remove(flavor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlavorExists(int id)
        {
            return _context.Flavors.Any(e => e.FlavorId == id);
        }
    }
}
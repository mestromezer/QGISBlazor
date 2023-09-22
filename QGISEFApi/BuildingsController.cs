using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QGISEFApi.Data;
using QGISEFApi.Models;
using System.Text.Json;

namespace QGISEFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        private readonly QGISEFApiContext _context;

        public BuildingsController(QGISEFApiContext context)
        {
            _context = context;
        }

        // GET: api/Buildings
        [HttpGet]
        public async Task<ActionResult<List<BuildingResponse>>> GetBuildings()
        {
          if (_context.Buildings == null)
          {
              return NotFound();
          }
          var queryResult = await _context.Buildings.ToListAsync();
            var response = new List<BuildingResponse>();
            foreach (var el in queryResult)
            {
                response.Add(el.ToBuildingResponse());
            }
          return Ok(response);
        }

        // GET: api/Buildings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
          if (_context.Buildings == null)
          {
              return NotFound();
          }
            var building = await _context.Buildings.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }

        // PUT: api/Buildings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(int id, Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            _context.Entry(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
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

        // POST: api/Buildings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
          if (_context.Buildings == null)
          {
              return Problem("Entity set 'QGISEFApiContext.Building'  is null.");
          }
            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
        }

        // DELETE: api/Buildings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            if (_context.Buildings == null)
            {
                return NotFound();
            }
            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuildingExists(int id)
        {
            return (_context.Buildings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

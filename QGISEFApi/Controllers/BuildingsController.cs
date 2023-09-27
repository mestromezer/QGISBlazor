using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using QGISEFApi.Data;
using QGISEFApi.Models;

namespace QGISEFApi
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly QGISEFApiContext _context;
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly ILogger<BuildingsController> _logger;

        public BuildingsController(QGISEFApiContext context, ILogger<BuildingsController> logger)
        {
            _context = context;
            var _serializer = GeoJsonSerializer.Create();
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        public async Task<ActionResult<List<Building>>> GetBuildings()
        {

            if (_context.Buildings == null)
            {
                return NotFound();
            }
            return Ok(await _context.Buildings.ToListAsync());
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            if (_context.Buildings == null)
            {
                return NotFound();
            }
            return await _context.Buildings.FindAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(Building building)
        {
            try
            {
                var queryResult = await _context.Buildings.FindAsync(building.Id);
                queryResult.Geom = building.Geom;
                queryResult.Address = building.Address;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(building.Id))
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
        [Route("/")]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            if (_context.Buildings == null)
            {
                return Problem("Entity set 'QGISEFApiContext.Building'  is null.");
            }

            // Даже если это выглядит смешно, то мне на этом этапе уже было совсем не смешно 
            // building.Geom.AsText() абсолютно никак не рабочая схема)
            // building.Geom.Buffer(0).AsText()) почему-то работает)))))))))))))))))))))))))))))))))))))))))))))))
            _context.Database.ExecuteSqlRaw(
            "INSERT INTO Buildings (geom,address) VALUES(@geom, @address)",
            new SqlParameter("geom", building.Geom.Buffer(0).AsText()),
            new SqlParameter("address", building.Address)
            );
            _context.SaveChanges();
            return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
        }

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

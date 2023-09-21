using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QGISEFCoreApi.Data;
using QGISEFCoreApi.Models;
using Microsoft.SqlServer.Types;

namespace QGISEFCoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly QGISEFCoreApiContext _context;
        private readonly ILogger<BuildingsController> _logger;

        public BuildingsController(QGISEFCoreApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/")]
        public async Task<List<Building>> GetBuilding()
        {
            if (_context.Building == null)
            {
                return null;
            }
            var tmp = await _context.Building.ToListAsync();
            return tmp;
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            if (_context.Building == null)
            {
                return NotFound();
            }
            var building = await _context.Building.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }

        [HttpPut]
        [Route("/")]
        public async Task<IActionResult> PutBuilding(int id, Building building)
        {
            if (id != building.ID)
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

        [HttpPost]
        [Route("/")]
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            if (_context.Building == null)
            {
                return Problem("Entity set 'QGISEFCoreApiContext.Building'  is null.");
            }
            _context.Building.Add(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuilding", new { id = building.ID }, building);
        }

        [HttpDelete]
        [Route("/{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            if (_context.Building == null)
            {
                return NotFound();
            }
            var building = await _context.Building.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            _context.Building.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("/{id}/bufferzone")]
        public async Task<string> CountBufferZone(int id)
        {
            try
            {
                Response.StatusCode = 200;
                var building = await _context.Building.FirstOrDefaultAsync((e) => e.ID == id);
                var geom = SqlGeometry.Parse(building.Geom);
                string area = geom.STBuffer(5).STArea().ToString();
                return area;
            }
            catch (Exception ex)
            {
                _logger.LogError("GET/[controller]/{id}/bufferzone occured error");
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        private bool BuildingExists(int id)
        {
            return (_context.Building?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

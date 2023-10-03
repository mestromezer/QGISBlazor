using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QGISEFApi.Data;
using QGISEFApi.Models;

namespace QGISEFApi;

[ApiController]
[Route("[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly QGISEFApiContext _context;
    //private readonly Newtonsoft.Json.JsonSerializer _serializer;
    private readonly ILogger<BuildingsController> _logger;

    public BuildingsController(QGISEFApiContext context, ILogger<BuildingsController> logger)
    {
        _context = context;
        if (_context == null)
        {
            throw new ArgumentNullException("Context was null");
        }
        _logger = logger;
    }

    [HttpGet]
    [Route("/")]
    public async Task<ActionResult<List<Building>>> GetBuildings()
    {
        if (_context.Buildings == null)
        {
            throw new Exception("Buildings was null.");
        }
        var queryReslt = await _context.Buildings.ToListAsync();
        if (queryReslt == null)
        { 
            return NotFound();
        }
        return Ok(queryReslt);
    }

    [HttpGet]
    [Route("/{id}")]
    public async Task<ActionResult<Building>> GetBuilding(int id)
    {
        if (_context.Buildings == null)
        {
            throw new Exception("Buildings was null.");
        }
        var queryResult = await _context.Buildings.FindAsync(id);
        if (queryResult == null) 
        { 
            return NotFound(); 
        }
        return Ok(queryResult);
    }

    [HttpPut("/")]
    public async Task<IActionResult> PutBuilding(Building building)
    {
        if (_context.Buildings == null)
        {
            throw new Exception("Buildings was null.");
        }
        try
        {
            var queryResult = await _context.Buildings.FindAsync(building.Id);
            if (queryResult == null) 
            {
                return NotFound();
            }
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
        if (building == null)
        {
            throw new Exception("Post method have got null building.");
        }
        await _context.Database.ExecuteSqlRawAsync(
            "INSERT INTO Buildings (geom,address) VALUES(@geom, @address)",
            new SqlParameter("geom", building.Geom.Buffer(0).AsText()),
            new SqlParameter("address", building.Address)
        );
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBuilding(int id)
    {
        if (_context.Buildings == null)
        {
            throw new Exception("Context was null.");
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
    [HttpGet]
    [Route("/{id}/bufferzone")]
    public async Task<ActionResult<string>> CountBufferZone(int id)
    {
        try
        {
            var building = await _context.FindAsync<Building>(id);
            if (building == null) 
            { 
                return NotFound(); 
            }
            var area = Convert.ToString(building.Geom.Buffer(5).Area);
            return Ok(area);
        }
        catch (Exception ex)
        {
            _logger.LogError($"GET/[controller]/{id}/bufferzone occured error");
            _logger.LogError(ex.Message);
            throw;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Types;
using QGISDirectDatabaseConnectionApi.Models;

namespace QGISDirectDatabaseConnectionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly ILogger<BuildingsController> _logger;
        private readonly Services.DbContext _connection;
        public BuildingsController(ILogger<BuildingsController> logger, Services.IDbContext connection)
        {
            _logger = logger;
            _connection = (Services.DbContext)connection;
        }
        [HttpGet]
        [Route("/")]
        public async Task<List<Building>> GetAll()
        {
            try
            {
                return await _connection.GetItems();
            }
            catch (Exception ex)
            {
                _logger.LogError("GET/[controller] occured error");
                throw ex;
            }
        }
        [HttpGet]
        [Route("/{id}")]
        public async Task<Building> GetOne(int id)
        {
            try
            {
                Response.StatusCode = 200;
                return await _connection.GetItem(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("GET/[controller]/{id} occured error");
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpPost]
        [Route("/")]
        public async Task Add(Building newbie)
        {
            try
            {
                Response.StatusCode = 201;
                await _connection.AddItem(newbie);
            }
            catch (Exception ex)
            {
                _logger.LogError("POST/[controller] occured error");
                throw ex;
            }
        }
        [HttpDelete]
        [Route("/{id}")]
        public async Task Delete(int id)
        {
            try
            {
                Response.StatusCode = 204;
                await _connection.DeleteItem(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("DELETE/[controller]/{id} occured error");
                throw ex;
            }
        }
        [HttpPut]
        [Route("/")]
        public async Task Update(Building newbie)
        {
            try
            {
                Response.StatusCode = 204;
                await _connection.UpdateItem(newbie);
            }
            catch (Exception ex)
            {
                _logger.LogError("Put/[controller] occured error");
                throw ex;
            }
        }
        [HttpGet]
        [Route("/{id}/bufferzone")]
        public async Task<string> CountBufferZone(int id)
        {
            try 
            {
                Response.StatusCode = 200;
                var building = await _connection.GetItem(id);
                var geom = SqlGeometry
                    .Parse(building.Geom.ToString()
                    );
                string area = geom.STBuffer(5).STArea().ToString();
                return area;
            }
            catch(Exception ex) 
            {
                _logger.LogError("GET/[controller]/{id}/bufferzone occured error");
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace QGISDataApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QGISController : ControllerBase
    {
        private readonly ILogger<QGISController> _logger;
        private readonly Services.DbConnection _connection;
        public QGISController(ILogger<QGISController> logger, Services.IDbConnection connection)
        {
            _logger = logger;
            _connection = (Services.DbConnection)connection;
        }
        [HttpGet]
        public async Task<List<Building>> GetAll()
        {
            try
            {
                //return JsonConvert.SerializeObject(await _connection.GetItems());
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
    }
}
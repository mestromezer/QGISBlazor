using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QGISDataApi.Services;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task<List<Building>> Index()
        {
            try
            {
                Response.StatusCode = 200;
                //var jsonResponse = JsonSerializer.Serialize(await _connection.GetItems());
                return await _connection.GetItems();
            }
            catch (Exception ex) 
            {
                _logger.LogError("./Index occured error");
                throw ex; 
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QGISApi.Data;
using QGISApi.Models;
using NetTopologySuite.IO;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace QGISApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingDbContext _context;
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly ILogger<BuildingController> _logger;

        public BuildingController(BuildingDbContext context, ILogger<BuildingController> logger)
        {
            _context = context;
            var _serializer = GeoJsonSerializer.Create();
            _logger = logger;
        }
        // GET: BuildingController
        [HttpGet]
        [Route("/")]
        public async  Task<ActionResult<List<Building>>> Index()
        {
            if (_context.Buildings == null)
            {
                return NotFound();
            }
            string json;
            var serializer = GeoJsonSerializer.Create();
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonWriter, queryResult);
                json = stringWriter.ToString();
            }
            return Ok(json);
        }
        /*
        // GET: BuildingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BuildingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuildingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BuildingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BuildingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BuildingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BuildingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
